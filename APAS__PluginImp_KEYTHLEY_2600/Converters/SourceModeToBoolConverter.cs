using System;
using System.Globalization;
using System.Windows.Data;

namespace APAS.Plugin.KEYTHLEY.SMU2600.Converters
{
    internal class SourceModeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return false;

            if (value is not SourceModeEnum mode)
                return false;

            var indicatorType = parameter.ToString();
            if (indicatorType == "ISource")
                return mode == SourceModeEnum.ISource;
            if (indicatorType == "VSource")
                return mode == SourceModeEnum.VSource;

            return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
