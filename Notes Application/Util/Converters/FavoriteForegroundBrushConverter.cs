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
    public class FavoriteForegroundBrushConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isFavorite = (bool)value;
            if(isFavorite)
            {
                return new SolidColorBrush(Colors.Goldenrod);
            }
            else
            {
                return new SolidColorBrush(Colors.LightSlateGray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
