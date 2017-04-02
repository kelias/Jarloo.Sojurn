using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Jarloo.Sojurn.Converters
{
    public class ShowUnwatchedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var i = (int) value;
                return i > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}