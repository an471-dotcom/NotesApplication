using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Notes_Application.Converters
{
    internal class DateConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string day;

            DateTime dt = (DateTime)value;
            long elapsedTicks = DateTime.Now.Ticks - dt.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            if (elapsedSpan.Days > 365)
            {
                day = dt.ToString("MMM dd, yyyy");
            }
            else if(elapsedSpan.Days >1)
            {
                day = dt.ToString("MMM dd");
            }
            else if(elapsedSpan.Days ==1)
            {
                day = "Yesterday";
            }
            else
            {
                day = "Today";
            }
            return day;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
