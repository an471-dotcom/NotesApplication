using Notes_Application.Utility_Classes;
using Notes_Application.ViewModel.Models;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Domain.UseCase.CommentUseCase;
using Notes_Library.Domain.UseCase.CommentUseCases;
using Notes_Library.Model.BussinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.Media.Protection.PlayReady;
using Windows.Networking.NetworkOperators;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using static Notes_Application.ViewModel.NoteDisplayUserControlViewModel;

namespace Notes_Application.ViewModel
{
    public class CommentsViewUserControlViewModel : INotifyPropertyChanged
    {
        private NotesVobj _note;
        public NotesVobj Note
        {
            get => _note;
            set
            {
                if (value != _note)
                {
                    _note = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Note)));

                }
            }
        }

        private string _currentUserName;
        public string CurrentUserName
        {
            get => _currentUserName;
            set
            {
                if(value != _currentUserName)
                {
                    _currentUserName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentUserName)));
                }
            }
        }

        private string _commentText;
        public string CommentText
        {
            get => _commentText;
            set
            {
                _commentText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentText)));
            }
        }
        private ObservableCollection<CommentBobj> _comments;
        public ObservableCollection<CommentBobj> Comments

        {
            get => _comments;
            set
            {
                if(value != _comments)
                {
                    _comments = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Comments)));
                }
            }
        }

        private Visibility _showReplyingTo = Visibility.Collapsed;
        public Visibility ShowReplyingTo
        {
            get => _showReplyingTo;
            set
            {
                _showReplyingTo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowReplyingTo)));
            }
        }

        private Visibility _commentsPanelVisibility = Visibility.Visible;
        public Visibility CommentsPanelVisibility
        {
            get => _commentsPanelVisibility;
            set
            {
                if(value != _commentsPanelVisibility)
                {
                    _commentsPanelVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentsPanelVisibility)));
                }
            }
        }

        private Visibility _replyPanelVisibility = Visibility.Collapsed;
        public Visibility ReplyPanelVisibility
        {
            get => _replyPanelVisibility;
            set
            {
                if(value != _replyPanelVisibility)
                {
                    _replyPanelVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReplyPanelVisibility)));
                }
            }
        }

        private ReplyBobj _CurrentReplyComment;
        public ReplyBobj CurrentReplyComment
        {
            get => _CurrentReplyComment;
            set
            {
                if (value != _CurrentReplyComment)
                {
                    _CurrentReplyComment = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentReplyComment)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void SetNoteData(NotesVobj note)
        {
            Note = note;

            if (Note != null)
            {
               Comments = new ObservableCollection<CommentBobj>(Note.NoteComments);
            }
        }

        private CommentBobj _selectedComment;
        public CommentBobj SelectedComment

        {
            get => _selectedComment;
            set
            {
                if (value != _selectedComment)
                {
                    _selectedComment = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedComment)));
                }


            }
        }

        public ObservableCollection<ReplyBobj> _replies;
        public ObservableCollection<ReplyBobj> Replies
        {
            get => _replies;
            set
            {
                if(value!= _replies)
                {
                    _replies = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Replies)));
                }
            }
        }

        private string _placeholderText ;
        public string PlaceholderText
        {
            get => _placeholderText;
            set
            {
                if(value != _placeholderText)
                {
                    _placeholderText = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(PlaceholderText)));
                }
            }
        }

        private Visibility _backButtonVisibility = Visibility.Collapsed;
        public Visibility BackButtonVisibility
        {
            get => _backButtonVisibility;
            set
            {
                if(value!= _backButtonVisibility)
                {
                    _backButtonVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackButtonVisibility)));
                }
            }
        }

        private string _titleText;
        public string TitleText

        {
            get => _titleText;
            set
            {
                if(value != TitleText)
                {
                    _titleText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitleText)));
                }
            }
        }

        private int _currentReplyId;
        ResourceLoader resourceLoader;


        public CommentsViewUserControlViewModel()
        {
            CurrentUserName = UserManager.CurrentUser.UserName;
            resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            _titleText = resourceLoader.GetString("CommentText");
            _placeholderText = resourceLoader.GetString("CommentPlaceholderText");
        }

        public void CloseReplyingToPanel()
        {
            ShowReplyingTo = Visibility.Collapsed;
            _currentReplyId = SelectedComment.Id;

        }

        public void SetSelectedComment(CommentBobj comment)
        {
            HideCommentsPanel();
            SelectedComment = comment;
            _currentReplyId = comment.Id;
            Replies = new ObservableCollection<ReplyBobj>(comment.Replies);
            ShowReplyingTo = Visibility.Collapsed;
        }

        public void SetReplyComment(ReplyBobj reply)
        {
            CurrentReplyComment = reply;
            _currentReplyId = reply.Id;
            ShowReplyingTo = Visibility.Visible;
        }

        public void OnCommentAdded(CommentBobj comment)
        {
            CommentText = string.Empty;
            Note.NoteComments.Insert(0, comment);
            Comments.Add(comment);

            
        }
        public void OnReplyAdded(ReplyBobj reply)
        {
            CommentText = string.Empty;
            var comment = Note.NoteComments.FirstOrDefault(c => c.Id == reply.ParentId);
            comment.Replies.Add(reply);
            Replies.Add(reply);
            _currentReplyId = SelectedComment != null ? SelectedComment.Id : 0;
            ShowReplyingTo = Visibility.Collapsed;
        }

        public void HideCommentsPanel()
        {
            CommentsPanelVisibility = Visibility.Collapsed;
            ReplyPanelVisibility = Visibility.Visible;
            CommentText = string.Empty;
            PlaceholderText = resourceLoader.GetString("ReplyPlaceholderText");
            TitleText = resourceLoader.GetString("ReplyText");
            BackButtonVisibility = Visibility.Visible;
        }
        public void ShowCommentPanel()
        {
            CommentsPanelVisibility = Visibility.Visible;
            ReplyPanelVisibility = Visibility.Collapsed;
            PlaceholderText = resourceLoader.GetString("CommentPlaceholderText");
            CommentText = string.Empty;
            SelectedComment = null;
            TitleText = resourceLoader.GetString("CommentText");
            BackButtonVisibility = Visibility.Collapsed;

        }

        public void OnCommentDeleted(int commentId)
        {
            Note.NoteComments.Remove(Note.NoteComments.FirstOrDefault(c => c.Id == commentId));
            var comment = Comments.FirstOrDefault(c => c.Id == commentId);
            Comments.Remove(comment);
            SelectedComment = null;
            ShowCommentPanel();
            
        }

        public void OnReplyDeleted(int parentId,int replyId)
        {
            var replies = Note.NoteComments.FirstOrDefault(c => c.Id == parentId).Replies;

            if(replies.Count() == 0)
            {
                return;
            }
            var replyReplies = replies.Where(r => r.CommentedTo == replyId);

            if(replyReplies.Count() == 0)
            {
                
                replies.Remove(replies.FirstOrDefault(r => r.Id == replyId));
                Replies.Remove(Replies.FirstOrDefault(r => r.Id == replyId));
                return;
            }

            foreach(var reply in replyReplies.ToList())
            {
                OnReplyDeleted(parentId,reply.Id);
            }
            replies.Remove(replies.FirstOrDefault(r => r.Id == replyId));
            Replies.Remove(Replies.FirstOrDefault(r => r.Id == replyId));
        }

        public void AddComment()
        {
            if (!String.IsNullOrWhiteSpace(CommentText))
            {

                if (SelectedComment != null)
                {
                    new AddReplyUseCase(new AddReplyRequest
                    {
                        ReplyText = CommentText,
                        RepliedTo = _currentReplyId,
                        ParentId = SelectedComment.Id,
                        UserId = UserManager.CurrentUser.UserId,

                    }, new AddReplyPresenterCallback(this)).Execute();
                }
                else
                {
                    new AddCommentUseCase(new AddCommentRequest
                    {
                        CommentedTo = Note.NoteId,
                        UserId = UserManager.CurrentUser.UserId,
                        CommentText = CommentText.Trim(),
                    }, new AddCommentPresenterCallback(this)).Execute();
                }
            }
            
        }
        public void DeleteComment(int commentId)
        {
            new DeleteCommentUseCase(new DeleteCommentRequest
            {
                CommentId = commentId,
            }, new DeleteCommentPresenterCallback(this)).Execute();
        }

        public void DeleteReply(int replyId)
        {
            new DeleteReplyUseCase(new DeleteReplyRequest
            {
                ReplyId = replyId,

            }, new DeleteReplyPresenterCallback(this)).Execute();
        }

        public class AddCommentPresenterCallback : IPresenterCallback<AddCommentResponse>
        {
            private CommentsViewUserControlViewModel _viewModel;
            public AddCommentPresenterCallback(CommentsViewUserControlViewModel viewModel)
            {
                _viewModel = viewModel;
            }
            public async void OnError(Exception e)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>
                {
                    await new MessageDialog(e.Message).ShowAsync();

                });
            }

            public async void OnSuccess(AddCommentResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   _viewModel.OnCommentAdded(result.Comment);

               });
            }
        }

        private class DeleteCommentPresenterCallback : IPresenterCallback<DeleteCommentResponse>
        {
            private CommentsViewUserControlViewModel _viewModel;

            public DeleteCommentPresenterCallback(CommentsViewUserControlViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public async void OnError(Exception e)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>
                {
                    await new MessageDialog(e.Message).ShowAsync();

                });
            }

            public async void OnSuccess(DeleteCommentResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   _viewModel.OnCommentDeleted(result.DeletedCommentId);
               });
            }
        }

        public class AddReplyPresenterCallback : IPresenterCallback<AddReplyResponse>
        {
            private CommentsViewUserControlViewModel _viewModel;

            public AddReplyPresenterCallback(CommentsViewUserControlViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public async void OnError(Exception e)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>
                {
                    await new MessageDialog(e.Message).ShowAsync();

                });
            }

            public async void OnSuccess(AddReplyResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   _viewModel.OnReplyAdded(result.Reply);

               });
            }
        }

        public class DeleteReplyPresenterCallback : IPresenterCallback<DeleteReplyResponse>
        {
            private CommentsViewUserControlViewModel _viewModel;

            public DeleteReplyPresenterCallback(CommentsViewUserControlViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public async void OnError(Exception e)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>
                {
                    await new MessageDialog(e.Message).ShowAsync();

                });
            }

            public async void OnSuccess(DeleteReplyResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
             () =>
             {
                 _viewModel.OnReplyDeleted(result.DeletedReplyParentId, result.DeletedReplyId);
             });
            }
        }

    }
}
