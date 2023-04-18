using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Notes_Application.Util.Converters
{
    public class StringToImageConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return XamlBindingHelper.ConvertValue(typeof(ImageSource), $"/Assets/NotebookCovers/{value}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
