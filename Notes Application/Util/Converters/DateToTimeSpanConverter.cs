using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Notes_Application.Converters
{
    public class DateToTimeSpanConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = (DateTime)value;
            var ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 60)
            {
                return ts.Seconds + "s";
            }
            if (delta < 60 * 2)
            {
                return "1m";
            }
            if (delta < 45 * 60)
            {
                return ts.Minutes + "m";
            }
            if (delta < 90 * 60)
            {
                return "1h";
            }
            if (delta < 24 * 60 * 60)
            {
                return ts.Hours + " h";
            }
            if (delta < 48 * 60 * 60)
            {
                return "1d";
            }
            if (delta < 30 * 24 * 60 * 60)
            {
                return ts.Days + " d";
            }
            if (delta < 12 * 30 * 24 * 60 * 60)
            {
                int months = (int)(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "1 mon" : months + " mon";
            }
            int years = (int)(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "1y ago" : years + " y";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
