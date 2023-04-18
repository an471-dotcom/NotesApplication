using Notes_Application.Presenter.ViewModel;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Notes_Application.Presenter.NoteView.ContentDialogs
{
    public sealed partial class UpdateTagDialog : ContentDialog
    {
        private UpdateTagViewModel _viewModel;
        public UpdateTagDialog()
        {
            this.InitializeComponent();
            _viewModel = new UpdateTagViewModel();
        }

       

       

        public Tag CurrentTag;


       
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _viewModel.UpdateTag();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var color = ((SolidColorBrush)e.ClickedItem).Color;
            _viewModel.TagColor = color.ToString();
            myColorButton.Flyout.Hide();
        }

        private void myColorButton_Click(Microsoft.UI.Xaml.Controls.SplitButton sender, Microsoft.UI.Xaml.Controls.SplitButtonClickEventArgs args)
        {
            var border = (Border)sender.Content;
            var color = ((Windows.UI.Xaml.Media.SolidColorBrush)border.Background).Color;
  
            
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.SetTagData(CurrentTag);
        }
    }
}
