using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace APAS.Plugin.KEYTHLEY.SMU2600.Converters
{
    internal class SourceOutputToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return false;

            if (value is not bool output)
                return false;

            var indicatorType = parameter.ToString();
            if (indicatorType == "ON")
                return output;
            if (indicatorType == "OFF")
                return !output;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
