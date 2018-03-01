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
    public class TotalIVEnumToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IVLevel level = (IVLevel)value;
            switch (level)
            {
                case IVLevel.Low:
                    return "Below Average";
                case IVLevel.Medium:
                    return "Above Average";
                case IVLevel.High:
                    return "Great";
                case IVLevel.Max:
                    return "Excellent";
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
