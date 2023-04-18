using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Notes_Application.Presenter.ViewModel.Models;
using Notes_Application.Utility_Classes;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Notes_Application.Presenter.NoteView.TokenStyle
{
    public sealed class MyTokenizingTextBox : TokenizingTextBox
    {
        public MyTokenizingTextBox() { }
       
        public event TypedEventHandler<TokenizingTextBox, object> TokenItemClick;

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            var tokenitem = element as TokenizingTextBoxItem;


            if (tokenitem != null)
            {
                tokenitem.Tapped -= Tokenitem_Tapped;
                tokenitem.Tapped += Tokenitem_Tapped;
            }





        }

        private void Tokenitem_Tapped(object sender, TappedRoutedEventArgs e)
        {

            var tag = ((TokenizingTextBoxItem)sender).Content as Tag;

            TokenItemClick?.Invoke(this, tag);
        }


    }
}
