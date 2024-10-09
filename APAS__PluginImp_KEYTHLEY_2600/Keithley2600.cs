using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using APAS.Plugin.KEYTHLEY.SMU2600.Extensions;
using APAS.Plugin.KEYTHLEY.SMU2600.Models;
using APAS.Plugin.KEYTHLEY.SMU2600.Ports;
using APAS.Plugin.KEYTHLEY.SMU2600.Views;
using APAS.Plugin.Sdk.Base;
using APAS.ServiceContract.Wcf;
using Reporter = (APAS.Plugin.KEYTHLEY.SMU2600.Models.MonitorReporter a, APAS.Plugin.KEYTHLEY.SMU2600.Models.MonitorReporter b);

namespace APAS.Plugin.KEYTHLEY.SMU2600
{
    /// <inheritdoc />
    public class Keithley2600 : PluginMultiChannelMeasurableEquipment
    {
        #region Variables

        public event EventHandler OnCommShot;

        private readonly object _locker = new object();

        private const string PATTEN_CONTROL_PARAM_ON = @"^ON ([AB]|ALL)$";
        private const string PATTEN_CONTROL_PARAM_OFF = @"^OFF ([AB]|ALL)$"; 

        private IPort _commPort;

        /// <summary>
        /// how long it takes to wait between the two sampling points.
        /// </summary>
        private readonly int _pollingIntervalMs = 200;

        private Task _bgTask;
        private CancellationTokenSource _cts;
        private CancellationToken _ct;
        private bool _isInit;
        private readonly Configuration _config;
        private readonly IProgress<Reporter> _rtValuesUpdatedReporter;

        #endregion

        #region Constructors

        public Keithley2600(ISystemService apasService, string caption) 
            : base(Assembly.GetExecutingAssembly(), apasService, caption, 4, 
                new []{"MeasV_A", "MeasI_A", "MeasV_B", "MeasI_B" })
        {
            #region Configuration Reading

            _config = GetAppConfig();

            LoadConfigItem(_config, "ReadIntervalMillisec", out _pollingIntervalMs, 200);

            #endregion

            ChannelA = new SingleChannelViewModel
            {
                TurnOnCommand = new RelayCommand(_=>Output("A", true)),
                TurnOffCommand = new RelayCommand(_ => Output("A", false)),
            };

            ChannelB = new SingleChannelViewModel
            {
                TurnOnCommand = new RelayCommand(_ => Output("B", true)),
                TurnOffCommand = new RelayCommand(_ => Output("B", false)),
            };

            UserView = new SMU2600View()
            {
                DataContext = this
            };

            HasView = true;

            //! the progress MUST BE defined in the ctor since
            //! we operate the UI elements in the OnCommOneShot event.
            _rtValuesUpdatedReporter = new Progress<Reporter>(reporter =>
            {
                ChannelA.SourceMode = reporter.a.Mode;
                ChannelB.SourceMode = reporter.b.Mode;
                ChannelA.IsON = reporter.a.IsON;
                ChannelB.IsON = reporter.b.IsON;
                ChannelA.MeasuredA = reporter.a.MeasureA;
                ChannelB.MeasuredA = reporter.b.MeasureA;
                ChannelA.MeasuredV = reporter.a.MeasureV;
                ChannelB.MeasuredV = reporter.b.MeasureV;
                ChannelA.SourceALevel = reporter.a.SourceI;
                ChannelB.SourceALevel = reporter.b.SourceI;
                ChannelA.SourceVLevel = reporter.a.SourceV;
                ChannelB.SourceVLevel = reporter.b.SourceV;
            });
        }

        #endregion

        #region Properties

        public override string ShortCaption => "SMU2600";

        public override string Description => "吉时立2600系列SMU控制插件";


        public override bool IsInitialized
        {
            get => _isInit;
            protected set => SetProperty(ref _isInit, value);
        }

        public SingleChannelViewModel ChannelA { get; }

        public SingleChannelViewModel ChannelB { get; }

        #endregion

        #region Methods

        public sealed override async Task<object> Execute(object args)
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// Switch to the specific channel.
        /// </summary>
        /// <param name="param">[int] The specific channel.</param>
        /// <returns></returns>
        public sealed override async Task Control(string param)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("SMU2600未初始化。");

            if (Regex.IsMatch(param, PATTEN_CONTROL_PARAM_ON)) // "ON"
            {
                var m = Regex.Match(param, PATTEN_CONTROL_PARAM_ON);
                if (m.Success)
                {
                    if (m.Groups[1].Value == "A") // ON A
                    {
                        Output("A", true);
                    }
                    else if (m.Groups[1].Value == "B") // ON B
                    {
                        Output("B", true);
                    }
                    else if (m.Groups[1].Value == "ALL") // ON ALL
                    {
                        Output("A", true);
                        Output("B", true);
                    }
                    else
                        goto __param_err;
                }
                else
                {
                    goto __param_err;
                }
            }
            else if (Regex.IsMatch(param, PATTEN_CONTROL_PARAM_OFF)) // "OFF"
            {
                var m = Regex.Match(param, PATTEN_CONTROL_PARAM_OFF);
                if (m.Success)
                {
                    if (m.Groups[1].Value == "A") // OFF A
                    {
                       Output("A", false);
                    }
                    else if (m.Groups[1].Value == "B") // OFF B
                    {
                        Output("B", false);
                    }
                    else if (m.Groups[1].Value == "ALL") // OFF ALL
                    {
                        Output("A", false);
                        Output("B", false);
                    }
                    else
                        goto __param_err;

                }
                else
                {
                    goto __param_err;
                }
            }
            else
            {
                throw new ArgumentException($"无效的控制参数 [{param}]，请查看Usage以获取有效的参数列表。");
            }

            await Task.CompletedTask;
            return;

            __param_err:
            throw new ArgumentException("控制命令参数错误。", nameof(param));


        }

        public override void Dispose()
        {
            _stopBackgroundTask();

            // disable the Rx.
            Control("OFF ALL").Wait();

        }

        public override object Fetch(int channel)
        {
            // 通道0~3分别代表"MeasV_A", "MeasI_A", "MeasV_B", "MeasI_B"

            var ret = 0.0d;

            switch (channel)
            {
                case 0:
                    ret = Query<double>($"print(smua.measure.i())");
                    break;

                case 1:
                    ret = Query<double>($"print(smua.measure.v())");
                    break;

                case 2:
                    ret = Query<double>($"print(smub.measure.i())");
                    break;

                case 3:
                    ret = Query<double>($"print(smub.measure.v())");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(channel));
            }

            return ret;
        }

        private MonitorReporter GetReporter(string channel)
        {
            var smu = $"smu{channel}";
            var reporter = new MonitorReporter();

            // 查询输出模式
            var func = Query<string>($"print({smu}.source.func)");
            if(double.TryParse(func, out var numMode))
                reporter.Mode = (int)numMode == 0 ? SourceModeEnum.ISource : SourceModeEnum.VSource;

            // 查询电压、电流测量值
            var ret = Query<string>($"print({smu}.measure.iv())");
            var iv = ret.Split('\t');
            if (double.TryParse(iv[0], out var curr) && double.TryParse(iv[1], out var volt))
            {
                reporter.MeasureA = curr;
                reporter.MeasureV = volt;
            }

            // 查询电压和电流设置值。
            reporter.SourceI = Query<double>($"print({smu}.source.leveli)");
            reporter.SourceV = Query<double>($"print({smu}.source.levelv)");

            // 查询输出状态
            ret = Query<string>($"print({smu}.source.output)");
            if (double.TryParse(ret, out var output))
                reporter.IsON = (int)output == 1;
            return reporter;

        }

        public override object[] FetchAll()
        {
            try
            {

                var reporterA = GetReporter("a");
                var reporterB = GetReporter("b");
            
                _rtValuesUpdatedReporter.Report((reporterA, reporterB));

                Application.Current?.Dispatcher.Invoke(() =>
                {
                    OnCommShot?.Invoke(this, EventArgs.Empty);
                });

                // 多通道设备，通道0~3对应"MeasV_A", "MeasI_A"，"MeasV_B", "MeasI_B"
                return [reporterA.MeasureV, reporterA.MeasureA, reporterB.MeasureV, reporterB.MeasureA];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override object Fetch()
        {
            try
            {
                var ret = FetchAll();
                if(ret.Length > 1)
                    return ret[0];

                return null;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public override object DirectFetch()
        {
            return Fetch();
        }

        public override bool Init()
        {
            try
            {
                _stopBackgroundTask();

                IsInitialized = false;
                IsEnabled = false;

                Init2600();

                IsInitialized = true;
                IsEnabled = true;

                _startBackgroundTask(_rtValuesUpdatedReporter);

                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public override void StartBackgroundTask()
        {
            // Do nothing}
        }

        public override void StopBackgroundTask()
        {
            // Do nothing
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 初始化通讯端口。
        /// </summary>
        private void InitCommPort()
        {
            LoadConfigItem(_config, "PortType", out var strPortType, PortTypeEnum.Serial.ToString());
            var portType = PortFactory.ConvertToPortType(strPortType);
            if (portType == null)
                throw new InvalidCastException($"端口类型定义错误，请检查PortType设置项。");

            switch (portType)
            {
                case PortTypeEnum.Serial:
                    LoadConfigItem(_config, "PortName", out var portName, "COM1");
                    LoadConfigItem(_config, "BaudRate", out var baudRate, 9600);
                    lock (_locker)
                    {
                        _commPort = PortFactory.CreateSerialPort(portName, baudRate);
                    }
                    
                    break;

                case PortTypeEnum.GPIB:
                    LoadConfigItem(_config, "GPIBAddr", out var gpibAddr, 1);
                    lock (_locker)
                    {
                        _commPort = PortFactory.CreateGPIBPort(0, (byte)gpibAddr);
                    }
                    break;
            }

            _commPort?.Open();
        }

        private void Init2600()
        {

            InitCommPort();

            var ret = Query<string>("*IDN?");
            if (!ret.Contains("2602") && !ret.Contains("2612"))
                throw new Exception($"连接到端口{_commPort}的设备不是Keithley SMU2600。");

            var config = GetAppConfig();
            LoadConfigItem(config, "InitTspFile", out var initTspFileName, "");
            RunTspFile(initTspFileName);
        }

        /// <summary>
        /// 打开或关闭输出。
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="isON"></param>
        private void Output(string channel, bool isON)
        {
            if (channel != "A" && channel != "B")
                throw new ArgumentException("通道参数错误，请使用字母A或B。", nameof(channel));

            SendCommand(isON 
                ? $"smu{channel.ToLower()}.source.output=1" 
                : $"smu{channel.ToLower()}.source.output=0");
        }

        private void RunTspFile(string filename)
        {
            var config = GetAppConfig();
            var fullName = Path.Combine(Path.GetDirectoryName(config.FilePath) ?? string.Empty, filename);
            
            if(!File.Exists(fullName))
                throw new FileNotFoundException($"无法找到脚本文件{fullName}");

            using (var reader = new StreamReader(fullName))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    
                    //空行
                    if(string.IsNullOrEmpty(line))
                        continue;
                    
                    // 注释行
                    if(line.StartsWith("#"))
                        continue;

                    var command = line;
                    
                    // 移除行尾可能存在的注释
                    if(line.Contains("#"))
                        command = line.Substring(0, line.IndexOf('#'));
                    SendCommand(command);
                }
            }
            
            // 等待命令执行完成
            SendCommand("opc()");
            SendCommand("waitcomplete()");
            Query<string>("print(\"1\")");
        }
        
        private void SendCommand(string cmd)
        {
            lock (_locker)
            {
                try
                {
                    _commPort.Write(string.Concat(cmd, "\r\n"));
                }
                catch (NullReferenceException)
                {
                    throw new NullReferenceException("可能还未连接到设备。");
                }
                
                Thread.Sleep(10);
            }
        }

        private T Query<T>(string cmd)
        {
            lock (_locker)
            {
                _commPort.Write(cmd);
                var ret = _commPort.ReadAscii();
                return ret.ConvertTo<T>();
            }
        }

        private void _startBackgroundTask(IProgress<Reporter> progress = null)
        {
            if (_bgTask == null || _bgTask.IsCompleted)
            {
                _cts = new CancellationTokenSource();
                _ct = _cts.Token;

                _bgTask = Task.Run(() =>
                {
                    // wait for 2s to ensure the UI is initialized completely.
                    Thread.Sleep(2000);

                    while (true)
                    {
                        try
                        {
                            Fetch();
                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        if (_ct.IsCancellationRequested)
                            return;

                        Thread.Sleep(_pollingIntervalMs);

                        if (_ct.IsCancellationRequested)
                            return;
                    }
                }, _ct);
            }
        }

        private void _stopBackgroundTask()
        {
            if (_bgTask != null)
            {
                // 结束背景线程
                _cts?.Cancel();

                //! 延时，确保背景线程正确退出
                Thread.Sleep(500);

                _bgTask = null;
            }

            IsInitialized = false;
            IsEnabled = false;
        }

        #endregion

        #region Commands

        /// <summary>
        /// Re-connect to the keithley 2602B
        /// </summary>
        public RelayCommand ReConnCommand
        {
            get
            {
                return new RelayCommand(x =>
                {
                    try
                    {
                        _stopBackgroundTask();
                        lock (_locker)
                        {
                            _commPort.Close();
                            _commPort?.Dispose();
                        }
                        
                        Init();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"无法连接2606B，{ex.Message}", "错误",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        /// <summary>
        /// 打开输出
        /// </summary>
        public RelayCommand OutputOnCommand
        {
            get
            {
                return new RelayCommand(ch =>
                {
                    try
                    {
                        Control($"ON {ch}").Wait();
                    }
                    catch (AggregateException ex)
                    {
                        var errMsg = new StringBuilder();
                        ex.Flatten().Handle(e =>
                        {
                            errMsg.AppendLine(e.Message);
                            return true;
                        });

                        MessageBox.Show($"无法打开输出，{errMsg}", "错误",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"无法打开输出，{ex.Message}", "错误",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        #endregion
    }
}
