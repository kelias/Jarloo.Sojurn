using System;
using System.Globalization;
using System.Windows.Data;

namespace Jarloo.Sojurn.Converters
{
    public class NumberExtensionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var num = (int) value;

            if (num == 1) return $"{num}st";
            if (num ==2) return $"{num}nd";
            if (num ==3) return $"{num}rd";
            return $"{num}th";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}