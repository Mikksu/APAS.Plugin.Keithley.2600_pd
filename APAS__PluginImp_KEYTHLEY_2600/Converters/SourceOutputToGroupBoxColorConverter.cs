using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace APAS.Plugin.KEYTHLEY.SMU2600.Converters
{
    internal class SourceOutputToGroupBoxColorConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // default border color: #FFD5DFE5

            if(parameter == null)
                return null;

            if (value is true)
            {
                if(parameter.ToString() == "Border")
                    return new SolidColorBrush(Colors.Green);
                if(parameter.ToString() == "Background")
                    return new LinearGradientBrush(
                        Color.FromArgb(0x26, 0x0, 0xFF, 0x0),
                        Color.FromArgb(0x0, 0x0, 0x0, 0x0),
                        new Point(0, 0.5), new Point(1, 0.5));
            }
            else
            {
                if (parameter.ToString() == "Border")
                    return new SolidColorBrush(Color.FromArgb(0xff, 0xD5, 0xDF, 0xE5));
                if (parameter.ToString() == "Background")
                    return new SolidColorBrush(Colors.Transparent);

            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
