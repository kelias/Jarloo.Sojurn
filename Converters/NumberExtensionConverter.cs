using System;
using System.Globalization;
using System.Windows.Data;

namespace Jarloo.Sojurn.Converters;

public class NumberExtensionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var num = (int)value;

        return num switch
        {
            1 => $"{num}st",
            2 => $"{num}nd",
            3 => $"{num}rd",
            _ => $"{num}th"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}