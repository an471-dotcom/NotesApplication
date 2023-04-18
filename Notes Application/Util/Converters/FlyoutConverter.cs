using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Notes_Application.Converters
{
    internal class FlyoutConverter : DependencyObject,IValueConverter
    {
        
        public MenuFlyout NoteFlayoutMenu { get; set; }

        public MenuFlyout SharedNoteFlayoutMenu { get; set; }   

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isShared = (bool)value;
            if (isShared)
            {
                return SharedNoteFlayoutMenu;
            }
            else
            {
                return NoteFlayoutMenu;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
