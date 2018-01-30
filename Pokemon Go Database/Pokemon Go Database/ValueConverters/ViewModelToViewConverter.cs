using Pokemon_Go_Database.IOC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Pokemon_Go_Database.ValueConverters
{
    [ValueConversion(typeof(BaseViewModel), typeof(UserControl))]
    public class ViewModelToViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var viewModel = value as BaseViewModel;
            return viewModel != null ? IoCContainer.ResolveScreen(viewModel.GetType()) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
