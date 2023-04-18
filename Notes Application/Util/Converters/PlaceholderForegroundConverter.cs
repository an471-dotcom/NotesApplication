using Notes_Application.Utility_Classes;
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
    public class PlaceholderForegroundConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Color color;
            if(value is Color)
            {
                color = (Color)value;
            }
            else
            {
                color = UtilityClass.GetColorFromHex((string)value);
            }

            
            if (IsDark(color))
            {
                return UtilityClass.GetColorFromHex("#8CFFFFFF");
            }
            else
            {
                return UtilityClass.GetColorFromHex("#FF767676");
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            SolidColorBrush brush = (SolidColorBrush)value;
            return brush.Color;
        }
        bool IsDark(Color color)
        {
            double y = 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;
            return y < 128;
        }
    }

    public class PlaceholderForegroundBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            var color = UtilityClass.GetColorFromHex((string)value);

            if (IsDark(color))
            {
                return new SolidColorBrush(UtilityClass.GetColorFromHex("#8CFFFFFF"));
            }
            else
            {
                return new SolidColorBrush(UtilityClass.GetColorFromHex("#FF767676"));
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        bool IsDark(Color color)
        {
            double y = 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;
            return y < 128;
        }
    }

}
