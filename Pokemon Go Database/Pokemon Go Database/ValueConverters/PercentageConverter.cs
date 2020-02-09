using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Pokemon_Go_Database.ValueConverters
{
    class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string value_str = value.ToString();
            if (String.IsNullOrWhiteSpace(value_str)) return null;

            value_str = value_str.TrimEnd(culture.NumberFormat.PercentSymbol.ToCharArray());

            double result;
            if (Double.TryParse(value_str, out result))
            {
                return result / 100.0;
            }
            return value;
        }
    }
}
