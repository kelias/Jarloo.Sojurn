using System;
using System.Globalization;
using System.Windows.Data;
using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.Converters
{
    public class ShowOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var w = (int) value;

                return w > 0 ? 1 : 0.3;
            }
            catch
            {
                return 1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}