using Notes_Application.Utility_Classes;
using Notes_Application.ViewModel.Models;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Notes_Application.ViewModel
{
    public class ShareInfoUserControlViewModel : INotifyPropertyChanged
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

        private ObservableCollection<UserInfo> _sharedUser;
        public ObservableCollection<UserInfo> SharedUser
        {
            get => _sharedUser;

            set
            {
                if (value != _sharedUser)
                {
                    _sharedUser = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SharedUser)));

                }
            }
        }

        private List<string> _emailSuggestion;
        public List<string> EmailSuggestion
        {
            get => _emailSuggestion;
            set
            {
                _emailSuggestion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmailSuggestion)));
            }
        }

        private Visibility _sharePanelVisibility = Visibility.Collapsed;
        public Visibility SharePanelVisibity
        {
            get => _sharePanelVisibility;
            set
            {
                if (value != _sharePanelVisibility)
                {
                    _sharePanelVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SharePanelVisibity)));
                }
            }
        }

        private Visibility _sharedUserPanelVisibility = Visibility.Visible;
        public Visibility SharedUserPanelVisibility
        {
            get => _sharedUserPanelVisibility;
            set
            {
                if (value != _sharedUserPanelVisibility)
                {
                    _sharedUserPanelVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SharedUserPanelVisibility)));
                }
            }
        }

        private Visibility _reactionPanelVisibility = Visibility.Collapsed;
        public Visibility ReactionPanelVisibility
        {
            get => _reactionPanelVisibility;
            set
            {
                if (value != _reactionPanelVisibility)
                {
                    _reactionPanelVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReactionPanelVisibility)));
                }
            }
        }

        public ObservableCollection<string> SelectedEmailList = new ObservableCollection<string>();

        private ObservableCollection<ReactionBobj> _noteReactions;
        public ObservableCollection<ReactionBobj> NoteReactions
        {
            get => _noteReactions;
            set
            {
                if (value != _noteReactions)
                {
                    _noteReactions = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoteReactions)));
                }
            }
        }

        private string _reactionCount;
        public string ReactionCount
        {
            get => _reactionCount;
            set
            {
                if (value != _reactionCount)
                {
                    _reactionCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReactionCount)));
                }

            }
        }
        private List<UserInfo> UnSharedUsers { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void SetNoteData(NotesVobj note)
        {
            Note = note;

            if (Note != null)
            {
                SharedUser = new ObservableCollection<UserInfo>(Note.SharedUsers);
                NoteReactions = new ObservableCollection<ReactionBobj>(Note.NoteReactions);
                if (NoteReactions.Count() > 0)
                {
                    ReactionCount = NoteReactions.Count().ToString();
                }
                else
                {
                    ReactionCount = "";
                }
            }

            SharePanelVisibity = Visibility.Collapsed;
        }


        public bool VerifyEmail(string tokenText)
        {
            foreach (var user in UnSharedUsers)
            {
                if (user.Email == tokenText)
                {

                    return true;

                }
            }
            return false;
        }

        public void GetEmailSuggestion(string input)
        {
            var suggestionList = UnSharedUsers.Where(user => !SelectedEmailList.Contains(user.Email)).ToList();
            EmailSuggestion = suggestionList.Where(u => u.Email.Contains(input)).Select(u => u.Email).ToList();


        }
        public void OnSuggestionReceived(GetUserSuggestionResponse result)
        {

            UnSharedUsers = new List<UserInfo>(result.UserEmails);

        }

        public void OnNoteUnshared(int userId, IEnumerable<CommentBobj> comments)
        {
            Note.SharedUsers.Remove(Note.SharedUsers.FirstOrDefault(u => u.UserId == userId));
            var user = SharedUser.FirstOrDefault(u => u.UserId == userId);
            SharedUser.Remove(user);
            var reaction = Note.NoteReactions.FirstOrDefault(r => r.ReactedBy == userId);
            Note.NoteReactions.Remove(reaction);
            NoteReactions = new ObservableCollection<ReactionBobj>(Note.NoteReactions);
            Note.NoteComments = new List<CommentBobj>(comments);
            if (NoteReactions.Count() > 0)
            {
                ReactionCount = NoteReactions.Count().ToString();
            }
            else
            {
                ReactionCount = "";
            }

        }

        public void OnNoteShared(ShareNoteResponse result)
        {
            foreach (var user in result.SharedList)
            {
                Note.SharedUsers.Add(user);
                SharedUser.Add(user);
            }
            HideSharePanel();
            SelectedEmailList.Clear();


        }

        public void HideReactionPanel()
        {
            ReactionPanelVisibility = Visibility.Collapsed;
            SharedUserPanelVisibility = Visibility.Visible;
        }
        public void ShowReactionPanel()
        {
            ReactionPanelVisibility = Visibility.Visible;
            SharePanelVisibity = Visibility.Collapsed;
            SharedUserPanelVisibility = Visibility.Collapsed;
        }
        public void ShowSharePanel()
        {
            SharePanelVisibity = Visibility.Visible;
            SharedUserPanelVisibility = Visibility.Collapsed;
        }

        public void HideSharePanel()
        {
            SharePanelVisibity = Visibility.Collapsed;
            SharedUserPanelVisibility = Visibility.Visible;
        }


        public void GetUnSharedUsers()
        {

            new GetUserSuggestionUseCase(new GetUserSuggestionRequest
            {
                NoteId = Note.NoteId,
                UserId = UserManager.CurrentUser.UserId,
            }, new UserSuggestionPresenterCallback(this)).Execute();
        }

        public void UnshareNote(int userId)
        {
            new UnshareNoteUseCase(new UnshareNoteRequest
            {
                NoteId = Note.NoteId,
                UserId = userId,
            }, new UnshareNotePresenterCallback(this)).Execute();
        }

        public void ShareNote()
        {
            new ShareNoteUseCase(new ShareNoteRequest
            {
                NoteId = Note.NoteId,
                EmailList = SelectedEmailList,
            }, new ShareNotePresenterCallback(this)).Execute();

        }

        public class UserSuggestionPresenterCallback : IPresenterCallback<GetUserSuggestionResponse>
        {
            private ShareInfoUserControlViewModel _viewModel;
            public UserSuggestionPresenterCallback(ShareInfoUserControlViewModel viewModel)
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

            public async void OnSuccess(GetUserSuggestionResponse response)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _viewModel.OnSuggestionReceived(response);

                });
            }
        }

        public class UnshareNotePresenterCallback : IPresenterCallback<UnshareNoteResponse>
        {
            private ShareInfoUserControlViewModel _viewModel;
            public UnshareNotePresenterCallback(ShareInfoUserControlViewModel viewModel)
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

            public async void OnSuccess(UnshareNoteResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _viewModel.OnNoteUnshared(result.UserId, result.Comments);

                });
            }
        }

        public class ShareNotePresenterCallback : IPresenterCallback<ShareNoteResponse>
        {
            private ShareInfoUserControlViewModel _viewModel;
            public ShareNotePresenterCallback(ShareInfoUserControlViewModel viewModel)
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

            public async void OnSuccess(ShareNoteResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     _viewModel.OnNoteShared(result);

                 });
            }
        }

    }
}
