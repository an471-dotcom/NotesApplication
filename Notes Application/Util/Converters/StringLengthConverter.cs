using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Notes_Application.Converters
{
    internal class StringLengthConverter:IValueConverter
    {
        public int length { get; set; } = 30;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value== null) return "";

            string title = (string)value;
            return title.Length <= length ? title : title.Substring(0, length) + "...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
