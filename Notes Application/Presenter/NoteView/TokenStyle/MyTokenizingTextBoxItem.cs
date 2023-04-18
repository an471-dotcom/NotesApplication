using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Notes_Application.Presenter.NoteView.TokenStyle
{
    public sealed class MyTokenizingTextBoxItem : TokenizingTextBoxItem
    { 
        public MyTokenizingTextBoxItem() 
        { 
        
        }




        public SolidColorBrush TokenColor
        {
            get { return (SolidColorBrush)GetValue(TokenColorProperty); }
            set { SetValue(TokenColorProperty, value); }
        }

        
        public static readonly DependencyProperty TokenColorProperty =
            DependencyProperty.Register("TokenColor", typeof(SolidColorBrush), typeof(TokenizingTextBoxItem), new PropertyMetadata(new SolidColorBrush(Colors.Pink)));


        

        
    }
}
