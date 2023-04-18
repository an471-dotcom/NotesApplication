using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Notes_Application.Converters
{
    public class DateFormatConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            
            return ((DateTime)value).ToString("ddd MMM dd");

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
