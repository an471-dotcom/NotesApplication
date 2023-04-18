using Notes_Application.Utility_Classes;
using Notes_Application.ViewModel;
using Notes_Application.ViewModel.Models;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class CommentUserControl : UserControl, INotifyPropertyChanged
    {
        public CommentBobj Comment
        {
            get
            {
                return this.DataContext as CommentBobj;
            }
        }

        private Visibility _deleteButtonVisibility = Visibility.Collapsed;
        public Visibility DeleteButtonVisibility
        {
            get => _deleteButtonVisibility;
            set
            {
                if (value != _deleteButtonVisibility)
                {
                    _deleteButtonVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeleteButtonVisibility)));

                }
            }
        }

       

        public event Action<int> DeleteComment;

        public event Action<CommentBobj> ViewCommentReplies;

        public event PropertyChangedEventHandler PropertyChanged;

        public CommentUserControl()
        {
            this.InitializeComponent();

            this.DataContextChanged += (s, e) => Bindings.Update();
        }



        private void DeleteCommentButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteComment?.Invoke(Comment.Id);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(Comment!=null)
            {
                if (Comment.CommentedBy == UserManager.CurrentUser.UserId)
                {
                    DeleteButtonVisibility = Visibility.Visible;
                }
                else
                {
                    DeleteButtonVisibility = Visibility.Collapsed;
                }

                
            }

            


        }

        private void ReplyButton_Click(object sender, RoutedEventArgs e)
        {
            ViewCommentReplies?.Invoke(Comment);
        }
    }
}
