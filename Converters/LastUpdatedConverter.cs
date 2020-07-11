using System;
using System.Globalization;
using System.Windows.Data;

namespace Jarloo.Sojurn.Converters
{
    public class LastUpdatedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is DateTime date)) return "Never";

                if (date.Date == DateTime.Today) return "Today";

                if (date.Date == DateTime.Today.AddDays(-1)) return "Yesterday";

                var days = (DateTime.Today - date).Days;

                return $"{days} Day{(days == 1 ? "" : "s")} Ago";
            }
            catch
            {
                return "Never";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}