using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Pokemon_Go_Database.ValueConverters
{
    [ValueConversion(typeof(double), typeof(Brush))]
    class DPSValueToBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double number = (double)values[0];
            double min = 0;
            double max = 100;

            // Get the value limits from parameter
            try
            {
                min = (double)values[1];
                max = (double)values[2];
            }
            catch (Exception)
            {
                return Brushes.Transparent;
                //throw new ArgumentException("Parameter not valid. Enter in format: 'MinDouble|MaxDouble'");
            }

            if (max <= min)
            {
                return Brushes.Transparent;
                //throw new ArgumentException("Parameter not valid. MaxDouble has to be greater then MinDouble.");
            }

            if (number >= min && number <= max)
            {
                // Calculate color channels
                double range = (max - min) / 2;
                number -= max - range;
                double factor = 255 / range;
                double green = number < 0 ? number * factor : 255;
                double red = number > 0 ? (range - number) * factor : 255;

                // Create and return brush
                Color color = Color.FromRgb((byte)red, (byte)green, 0);
                SolidColorBrush brush = new SolidColorBrush(color);
                return brush;
            }

            // Fallback brush
            return Brushes.Transparent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
