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
    public sealed partial class AddReactionUserControl : UserControl
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
           DependencyProperty.Register("Note", typeof(NotesVobj), typeof(AddReactionUserControl), new PropertyMetadata(null,OnNotePropertyChanged));

        private static void OnNotePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is AddReactionUserControl cntrl)
            {
                cntrl.NoteVobjChanged((NotesVobj)e.NewValue);
            }
        }

        private void NoteVobjChanged(NotesVobj newValue)
        {
            _viewModel.SetNoteData(newValue);
        }

        private AddReactionUserControlviewmodel _viewModel;
        public AddReactionUserControl()
        {
            this.InitializeComponent();
            _viewModel = new AddReactionUserControlviewmodel();
            
        }

        private void ReactionList_ItemClick(object sender, ItemClickEventArgs e)
        {
            _viewModel.AddNoteReaction((ReactionIcon)e.ClickedItem);
            var flyout = ReactionsButton.Flyout;
            flyout.Hide();
        }

        private void ReactionsButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SetReaction();
        }
    }
}
