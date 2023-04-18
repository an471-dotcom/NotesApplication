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
    public class ColorShadeConverter : IValueConverter
    {
      
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Color color;

            if (value is Color)
            {
                color = (Color)value;
            }
            else
            {
                color = UtilityClass.GetColorFromHex((string)value);
            }
                
            var newColor = Color.FromArgb(color.A, (byte)(color.R * 0.7), (byte)(color.G * 0.7), (byte)(color.B * 0.7));
            return new SolidColorBrush(newColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            SolidColorBrush brush = (SolidColorBrush)value;
            return brush.Color;
        }
       
    }
}
