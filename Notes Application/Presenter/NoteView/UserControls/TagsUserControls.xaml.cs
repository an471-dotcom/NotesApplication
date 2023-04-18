using Notes_Application.Presenter.NoteView.ContentDialogs;
using Notes_Application.Presenter.ViewModel.Models;
using Notes_Application.Utility_Classes;
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
    public sealed partial class TagsUserControls : UserControl
    {



        public NotesVobj Note
        {
            get { return (NotesVobj)GetValue(NoteProperty); }
            set 
            { 
                SetValue(NoteProperty, value);
                _viewModel.SetNoteData(Note);
              
            }
        }

        
        public static readonly DependencyProperty NoteProperty =
            DependencyProperty.Register("Note", typeof(NotesVobj), typeof(TagsUserControls), new PropertyMetadata(null));


        private TagsUserControlViewModel _viewModel;

        public TagsUserControls()
        {
            this.InitializeComponent();
            _viewModel = new TagsUserControlViewModel();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.SetNoteData(Note);
           
        }

        private void TagsCollection_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.CheckCurrent() && args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var text = ((AutoSuggestBox)sender).Text;
                _viewModel.GetTagsSuggestion(text);

            }
        }

        private void TagsCollection_TokenItemRemoving(Microsoft.Toolkit.Uwp.UI.Controls.TokenizingTextBox sender, Microsoft.Toolkit.Uwp.UI.Controls.TokenItemRemovingEventArgs args)
        {
            var tag = args.Item as Tag;
            _viewModel.DeleteNoteTag(tag.Id);
            _viewModel.DeletedTag = tag;
        }

        private void TagsCollection_TokenItemAdded(Microsoft.Toolkit.Uwp.UI.Controls.TokenizingTextBox sender, object args)
        {
            var tag = (Tag)args;
            var noteTag = _viewModel.NoteTags.FirstOrDefault(t => t.Name == tag.Name);
            _viewModel.NoteTags.Remove(noteTag);
            _viewModel.AddTag(((Tag)args).Name);
        }

        private void TagsCollection_TokenItemAdding(Microsoft.Toolkit.Uwp.UI.Controls.TokenizingTextBox sender, Microsoft.Toolkit.Uwp.UI.Controls.TokenItemAddingEventArgs args)
        {
            args.Item = new TagVobj { Name = args.TokenText, UserId = UserManager.CurrentUser.UserId };
        }

        private async void TagsCollection_TokenItemClick(Microsoft.Toolkit.Uwp.UI.Controls.TokenizingTextBox sender, object args)
        {
            var updateTagDialog = new UpdateTagDialog() { CurrentTag = (TagVobj)args };
            await updateTagDialog.ShowAsync();
        }

        private void UpdateTagDialog_UpdateTag(Tag obj)
        {
            _viewModel.UpdateTag(obj);
        }

       
    }
}
