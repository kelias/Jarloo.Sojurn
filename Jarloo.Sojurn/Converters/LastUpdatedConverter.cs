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
                var date = value as DateTime?;

                if (date == null) return "Never";

                if (date.Value.Date == DateTime.Today) return "Today";

                var days = (DateTime.Today - date.Value).Days;

                return string.Format("{0} Day{1} Ago",days,days==1?"":"s");
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