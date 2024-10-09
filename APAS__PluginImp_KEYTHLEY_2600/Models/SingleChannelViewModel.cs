using APAS.CoreLib.Base;

namespace APAS.Plugin.KEYTHLEY.SMU2600.Models
{
    public class SingleChannelViewModel : BindableBase
    {
        #region properties


        private VoltUnitEnum _voltUnitSelected;

        /// <summary>
        /// 设置或返回电压单位。
        /// </summary>
        public VoltUnitEnum VoltUnitSelected
        {
            get => _voltUnitSelected;
            set => SetProperty(ref _voltUnitSelected, value);
        }

        private CurrentUnitEnum _currUnitSelected;

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
