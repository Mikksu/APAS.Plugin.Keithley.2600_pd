using System;
using System.Globalization;
using System.Windows.Data;

namespace APAS.Plugin.KEYTHLEY._2600_PD.Converters
{
    public class ReverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = bool.Parse(value.ToString());
            return !b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
