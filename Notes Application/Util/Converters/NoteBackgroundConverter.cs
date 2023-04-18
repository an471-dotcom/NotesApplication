using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace Notes_Application.Converters
{
    internal class NoteBackgroundConverter:IValueConverter
    {
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                Color color = (Color)value;
                if (IsDark(color))
                {
                    return new SolidColorBrush(Colors.Black);
                }
                else
                {
                    return new SolidColorBrush(Colors.White);
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
}
