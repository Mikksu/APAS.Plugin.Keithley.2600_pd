using APAS.CoreLib.Base;

namespace APAS.Plugin.KEYTHLEY.SMU2600.Models
{
    public class SingleChannelViewModel : BindableBase
    {

        #region properties

        private SourceModeEnum _mode = SourceModeEnum.ISource;

        /// <summary>
        /// 返回源输出模式。
        /// </summary>
        public SourceModeEnum SourceMode
        {
            get => _mode;
            internal set => SetProperty(ref _mode, value);
        }

        private bool _isON;

        /// <summary>
        /// 返回当前通道输出状态。
        /// </summary>
        public bool IsON
        {
            get => _isON;
            internal set => SetProperty(ref _isON, value);
        }


        private VoltUnitEnum _voltUnitSelected = VoltUnitEnum.V;

        /// <summary>
        /// 设置或返回电压单位。
        /// </summary>
        public VoltUnitEnum VoltUnitSelected
        {
            get => _voltUnitSelected;
            set => SetProperty(ref _voltUnitSelected, value);
        }

        private CurrentUnitEnum _currUnitSelected = CurrentUnitEnum.A;

        /// <summary>
        /// 设置或返回电流单位。
        /// </summary>
        public CurrentUnitEnum CurrentUnitSelected
        {
            get => _currUnitSelected;
            set => SetProperty(ref _currUnitSelected, value);
        }

        private double _measuredA;

        /// <summary>
        /// 返回测量电流值。
        /// </summary>
        public double MeasuredA
        {
            get => _measuredA;
            internal set => SetProperty(ref _measuredA, value);
        }

        private double _measuredV;

        /// <summary>
        /// 返回测量电压值。
        /// </summary>
        public double MeasuredV
        {
            get => _measuredV;
            internal set => SetProperty(ref _measuredV, value);
        }

        private double _sourceVLevel;

        /// <summary>
        /// 返回电压输出值。
        /// </summary>
        public double SourceVLevel
        {
            get => _sourceVLevel;
            internal set => SetProperty(ref _sourceVLevel, value);
        }

        private double _sourceALevel;

        /// <summary>
        /// 返回电流输出值。
        /// </summary>
        public double SourceALevel
        {
            get => _sourceALevel;
            internal set => SetProperty(ref _sourceALevel, value);
        }

        private RelayCommand _turnOnCommand;
        public RelayCommand TurnOnCommand
        {
            get => _turnOnCommand;
            set => SetProperty(ref _turnOnCommand, value);
        }

        private RelayCommand _turnOffCommand;
        public RelayCommand TurnOffCommand
        {
            get => _turnOffCommand;
            set => SetProperty(ref _turnOffCommand, value);
        }

        #endregion
    }
}
