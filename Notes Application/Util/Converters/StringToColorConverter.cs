using Notes_Application.Utility_Classes;
using Notes_Library.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Notes_Application.Converters
{
    public class StringToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string color = value as string;
            return UtilityClass.GetColorFromHex(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var color = (Color)value;

            return color.ToString();
            
        }
    }

    public class StringToBrushConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string color = value as string;
            return new SolidColorBrush(UtilityClass.GetColorFromHex(color));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
