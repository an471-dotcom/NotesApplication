using Notes_Application.ViewModel;
using Notes_Application.ViewModel.Models;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Notes_Application.UserControls
{
    public sealed partial class ShareInfoUserControl : UserControl
    {

        public NotesVobj Note
        {
            get { return (NotesVobj)GetValue(NoteProperty); }
            set
            {
                SetValue(NoteProperty, value);
               
            }
        }


        public static readonly DependencyProperty NoteProperty =
            DependencyProperty.Register("Note", typeof(NotesVobj), typeof(ShareInfoUserControl), new PropertyMetadata(null,OnNotePropertyChanged));

        private static void OnNotePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ShareInfoUserControl cntrl)
            {
                cntrl.NoteVobjChanged((NotesVobj)e.NewValue);
            }
        }

        private void NoteVobjChanged(NotesVobj newValue)
        {
           _viewModel.SetNoteData(newValue);
        }

        private ShareInfoUserControlViewModel _viewModel;



        public ShareInfoUserControl()
        {
            
            this.InitializeComponent();
              
            _viewModel = new ShareInfoUserControlViewModel();
       
            
        }

        private void TokenBoxEmail_TokenItemAdding(Microsoft.Toolkit.Uwp.UI.Controls.TokenizingTextBox sender, Microsoft.Toolkit.Uwp.UI.Controls.TokenItemAddingEventArgs args)
        {
            var isValid = _viewModel.VerifyEmail(args.TokenText);
            if (isValid)
            {
                args.Item = args.TokenText;
            }
            else
            {
                args.Cancel = true;
            }
        }

        private void TokenBoxEmail_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var input = ((AutoSuggestBox)sender).Text;
            _viewModel.GetEmailSuggestion(input);
        }

        private void TokenBoxEmail_TokenItemAdded(Microsoft.Toolkit.Uwp.UI.Controls.TokenizingTextBox sender, object args)
        {

        }

        private void ShareUserButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ShareNote();
        }

        private void AddMembersButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ShowSharePanel();
            _viewModel.GetUnSharedUsers();
            
        }

        private void UserInfoControl_UnshareNote(int obj)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.SetNoteData(Note);
            
            
        }

        private void UnShareButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.DataContext as UserInfo;
            _viewModel.UnshareNote(user.UserId);
           
        }

       

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.HideSharePanel();
        }

        private void ReactionsButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ShowReactionPanel();
        }

        private void CloseReactionsButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.HideReactionPanel();
        }
    }
}
