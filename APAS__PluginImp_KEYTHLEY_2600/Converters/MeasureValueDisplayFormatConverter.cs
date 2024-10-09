using System;
using System.Globalization;
using System.Windows.Data;

namespace APAS.Plugin.KEYTHLEY.SMU2600.Converters
{
    internal class MeasureValueDisplayFormatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return "Converter Error";
            
            if (values[0] is not double val)
                return "Converter Error";

            var output = 0.0d;
            if (values[1] is VoltUnitEnum unitV)
            {
                output = val * (int)unitV;
                output = Math.Round(output, GetNumberDigits((int)unitV) + 1);
            }
            else if (values[1] is CurrentUnitEnum unitA)
            {
                output = val * (int)unitA;
                output = Math.Round(output, GetNumberDigits((int)unitA) + 1);
            }
            else
            {
                return values[0];
            }

            return output.ToString(CultureInfo.InvariantCulture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 计算指定整数的位数。
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private int GetNumberDigits(int num)
        {
            return num.ToString().Length;
        }
    }
}
