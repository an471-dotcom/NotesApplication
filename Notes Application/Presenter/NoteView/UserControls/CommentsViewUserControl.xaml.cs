using Notes_Application.ViewModel;
using Notes_Application.ViewModel.Models;
using Notes_Library.Model.BussinessObjects;
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
    public sealed partial class CommentsViewUserControl : UserControl
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
            DependencyProperty.Register("Note", typeof(NotesVobj), typeof(CommentsViewUserControl), new PropertyMetadata(null,OnNotePropertyChanged));

        private static void OnNotePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is CommentsViewUserControl cntrl)
            {
                cntrl.NoteVobjChanged((NotesVobj)e.NewValue);
            }
        }

        private void NoteVobjChanged(NotesVobj newValue)
        {
            _viewModel.SetNoteData(newValue);
        }

        private CommentsViewUserControlViewModel _viewModel;
        public CommentsViewUserControl()
        {
            this.InitializeComponent();
            _viewModel = new CommentsViewUserControlViewModel();
            
        }

        private void CreateCommentButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddComment();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
           _viewModel.SetSelectedComment((e.ClickedItem) as CommentBobj);
        }

        private void DeleteCommentFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (MenuFlyoutItem)sender;
            var comment = item.DataContext as CommentBobj;
            _viewModel.DeleteComment(comment.Id);
        }

        private void CloseReplyButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CloseReplyingToPanel();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ShowCommentPanel();
        }

        private void CommentControl_DeleteComment(int commentId)
        {
            _viewModel.DeleteComment(commentId);
        }

        private void DeleteReplyButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (Button)sender;
            var reply = item.DataContext as ReplyBobj;
            _viewModel.DeleteReply(reply.Id);
        }

        private void ReplyButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (Button)sender;
            var reply = item.DataContext as ReplyBobj;
            _viewModel.SetReplyComment(reply);
        }

      

        private void CommentUserControl_ViewCommentReplies(CommentBobj obj)
        {
            _viewModel.SetSelectedComment(obj);
        }
    }
}
