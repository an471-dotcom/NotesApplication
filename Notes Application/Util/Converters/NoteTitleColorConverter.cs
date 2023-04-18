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
    internal class NoteTitleColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string colorString = (string)value;
            var color = GetColorFromHex(colorString);
            if (IsDark(color))
            {
                return new SolidColorBrush(Colors.WhiteSmoke);
            }
            else
            {
                return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private Windows.UI.Color GetColorFromHex(string colorString)
        {
            byte a = byte.Parse(colorString.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            byte r = byte.Parse(colorString.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(colorString.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(colorString.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
            return Windows.UI.Color.FromArgb(a, r, g, b);
        }
        bool IsDark(Color color)
        {
            double y = 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;
            return y < 128;
        }
    }
}
