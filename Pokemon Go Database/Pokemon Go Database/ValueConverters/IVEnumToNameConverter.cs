using IVLevel = Pokemon_Go_Database.Model.IVLevel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Pokemon_Go_Database.ValueConverters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class IVEnumToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IVLevel level = (IVLevel)value;
            switch (level)
            {
                case IVLevel.Low:
                    return "0-7";
                case IVLevel.Medium:
                    return "8-12";
                case IVLevel.High:
                    return "13-14";
                case IVLevel.Max:
                    return "15";
                default:
                    return "Unknown";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
