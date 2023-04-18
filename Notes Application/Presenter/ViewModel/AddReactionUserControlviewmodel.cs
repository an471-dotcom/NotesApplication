using Notes_Application.Utility_Classes;
using Notes_Application.ViewModel.Models;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Domain.UseCase.Reaction;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using static Notes_Application.ViewModel.NoteDisplayUserControlViewModel;

namespace Notes_Application.ViewModel
{
    public class AddReactionUserControlviewmodel : INotifyPropertyChanged
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
        private string _sharedUserEmail;
        public string SharedUserEmail
        {
            get => _sharedUserEmail;
            set
            {
                if (value != _sharedUserEmail)
                {
                    _sharedUserEmail = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SharedUserEmail)));
                }
            }
        }

        private string _sharedUserName;
        public string SharedUserName
        {
            get => _sharedUserName;
            set
            {
                _sharedUserName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SharedUserName)));
            }
        }
        private ReactionIcon _selectedReaction;
        public ReactionIcon SelectedReaction
        {
            get => _selectedReaction;

            set
            {
                if (value != _selectedReaction)
                {
                    _selectedReaction = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedReaction)));
                }
            }
        }

        private Visibility _showSelectedReaction;
        public Visibility ShowSelectedReaction
        {
            get => _showSelectedReaction;
            set
            {
                if (value != _showSelectedReaction)
                {
                    _showSelectedReaction = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowSelectedReaction)));
                }

            }
        }

        private string _reactionButtonText;
        public string ReactionButtonText
        {
            get => _reactionButtonText;
            set
            {
                if (value != _reactionButtonText)
                {
                    _reactionButtonText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReactionButtonText)));
                }
            }
        }
        public List<ReactionIcon> ReactionList = new List<ReactionIcon>()
        {
                new ReactionIcon{Name=Reactions.like, Glyph = "\U0001F44D"},
                new ReactionIcon{Name = Reactions.Heart ,Glyph="\u2665"},
                new ReactionIcon{Name = Reactions.Sad,Glyph="\U0001F625"},
                new ReactionIcon{Name = Reactions.Happy, Glyph = "\U0001F60A"},
                new ReactionIcon{Name = Reactions.Celebrate,Glyph="\U0001F44F"},

        };
        public event PropertyChangedEventHandler PropertyChanged;


        public void SetReaction()
        {
            if (Note?.IsSharedNote ?? false)
            {
                var userReaction = Note.NoteReactions.FirstOrDefault(r => r.ReactedBy == UserManager.CurrentUser.UserId);
                if (userReaction != null)
                {
                    SelectedReaction = ReactionList.FirstOrDefault(r => r.Name == userReaction.ReactionType);
                    ShowSelectedReaction = Visibility.Visible;
                    ReactionButtonText = userReaction.ReactionType.ToString();
                }
                else
                {
                    ShowSelectedReaction = Visibility.Collapsed;
                    ReactionButtonText = "Like";
                }

            }
            else
            {
                ShowSelectedReaction = Visibility.Collapsed;
            }
        }

        public void OnSharedUserReceived(GetUserResponse result)
        {
            SharedUserEmail = result.User.Email;
            SharedUserName = result.User.UserName;
        }
        public void SetNoteData(NotesVobj note)
        {
            Note = note;
            if (Note != null)
            {
                SetReaction();
                new GetUserUseCase(new GetUserRequest
                {
                    UserId = Note.CreatedBy,
                }, new GetSharedUserPresenterCallback(this)).Execute();
            }

        }

        public void OnReactionAdded(ReactionBobj reaction)
        {

            var addedReaction = Note.NoteReactions.FirstOrDefault(r => r.Id == reaction.Id);
            if (addedReaction != null)
            {
                addedReaction.ReactionType = reaction.ReactionType;
            }
            else
            {
                Note.NoteReactions.Add(reaction);
            }
            SelectedReaction = ReactionList.FirstOrDefault(r => r.Name == reaction.ReactionType);
            ShowSelectedReaction = Visibility.Visible;
            ReactionButtonText = reaction.ReactionType.ToString();
        }

        public void OnReactionRemoved(int reactionId)
        {
            SelectedReaction = null;
            var reaction = Note.NoteReactions.FirstOrDefault(r => r.Id == reactionId);
            Note.NoteReactions.Remove(reaction);
            ShowSelectedReaction = Visibility.Collapsed;
            ReactionButtonText = "Like";
        }
        public void AddNoteReaction(ReactionIcon reactionIcon)
        {
            var reaction = Note.NoteReactions.FirstOrDefault(r => r.ReactedBy == UserManager.CurrentUser.UserId);
            if (reactionIcon.Name == reaction?.ReactionType)
            {

                new RemoveReactionUseCase(new RemoveReactionRequest
                {
                    ReactionId = reaction.Id,

                }, new RemoveReactionPresenterCallback(this)).Execute();
            }
            else
            {
                new AddReactionUseCase(new AddReactionRequest
                {
                    ReactedBy = UserManager.CurrentUser.UserId,
                    ReactedTo = Note.NoteId,
                    ReactionType = reactionIcon.Name,

                }, new AddReactionPresenterCallback(this)).Execute();
            }

        }
        public class GetSharedUserPresenterCallback : IPresenterCallback<GetUserResponse>
        {
            private AddReactionUserControlviewmodel _viewModel;
            public GetSharedUserPresenterCallback(AddReactionUserControlviewmodel viewModel)
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

            public async void OnSuccess(GetUserResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     _viewModel.OnSharedUserReceived(result);

                 });
            }
        }
        public class AddReactionPresenterCallback : IPresenterCallback<AddReactionResponse>
        {
            private AddReactionUserControlviewmodel _viewModel;

            public AddReactionPresenterCallback(AddReactionUserControlviewmodel viewModel)
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

            public async void OnSuccess(AddReactionResponse response)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _viewModel.OnReactionAdded(response.Reaction);
                });
            }
        }

        public class RemoveReactionPresenterCallback : IPresenterCallback<RemoveReactionResponse>
        {
            private AddReactionUserControlviewmodel _viewModel;

            public RemoveReactionPresenterCallback(AddReactionUserControlviewmodel viewModel)
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

            public async void OnSuccess(RemoveReactionResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     _viewModel.OnReactionRemoved(result.RemovedReactionId);
                 });

            }
        }
    }
}
