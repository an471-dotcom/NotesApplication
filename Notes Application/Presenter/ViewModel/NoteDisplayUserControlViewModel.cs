using Notes_Application.UserControls;
using Notes_Application.Utility_Classes;
using Notes_Application.ViewModel.Models;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
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
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;


namespace Notes_Application.ViewModel
{
    public class NoteDisplayUserControlViewModel : INotifyPropertyChanged
    {
        private static NoteDisplayUserControlViewModel _instance;

        private NoteDisplayUserControlViewModel()
        {
            
        }

        public static NoteDisplayUserControlViewModel GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NoteDisplayUserControlViewModel();
            }
            return _instance;
        }

        private NotesVobj _note;

        public NotesVobj Note
        {
            get => _note;
            set
            {
                if(value != _note)
                {
                    _note = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(Note)));
                }
            }
        }

        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                if(value != _title)
                {
                    _title = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(Title)));
                }
            }
        }

        private string _noteColor = "#20202020";
        public string NoteColor 
        {
            get => _noteColor;
            set
            {
                if(value != _noteColor)
                {
                    _noteColor = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(NoteColor)));
                }
            }
        }
        private Visibility _toolbarVisibility;
        public Visibility ToolbarVisibility
        {
            get => _toolbarVisibility;
            set
            {
                if(value != _toolbarVisibility)
                {
                    _toolbarVisibility = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(ToolbarVisibility)));  
                } 
            }
        }
        
        private bool _textBoxReadOnly = false;
        public bool TextBoxReadOnly
        {
            get => _textBoxReadOnly;
            set
            {
                if(value != _textBoxReadOnly)
                {
                    _textBoxReadOnly = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextBoxReadOnly)));
                }
            }
        }
        private Visibility _noteVisibility = Visibility.Collapsed;
        public Visibility NoteVisibility
        {
            get => _noteVisibility;
            set
            {
                if (value != _noteVisibility)
                {
                    _noteVisibility = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(NoteVisibility)));
                }
            }
        }
        
        public INoteDisplayView NoteDisplayView { get; set; }  

        public event PropertyChangedEventHandler PropertyChanged;


       

        public void SetNoteData(NotesVobj note)
        {
            
            Note = note;

            if(Note != null)
            {
                NoteVisibility = Visibility.Visible;
                TextBoxReadOnly = false;
                NoteDisplayView.SetNoteContent(note.NoteContent);
                Title= note.Title;
                NoteColor= note.NoteColor;
                if(Note.IsSharedNote)
                {
                    ToolbarVisibility= Visibility.Collapsed;
                  
                    TextBoxReadOnly = true;
                }
                else
                {
                    ToolbarVisibility= Visibility.Visible;
                  
                    
                }

                
            }
            else
            {
                NoteVisibility = Visibility.Collapsed;
                
            }
            
        }

        public void OnNoteUpdated(NoteBobj note) 
        {
            Note.Title = note.Title;
            Note.NoteColor = note.NoteColor;
            Note.NoteContent = note.NoteContent;
            Note.UpdatedAt= note.UpdatedAt;
            NoteDisplayView.SetNoteContent(note.NoteContent);
            Title = note.Title;
            NoteColor = note.NoteColor;
         
        }

        public void NoteFavorited(int noteId)
        {
            Note.IsFavorite= true;
           
          
        }
        public void NoteUnFavorited(int noteId)
        {
            Note.IsFavorite= false;
            
        }

        public void ToggleFavoriteStatus()
        {
            if(Note.IsFavorite)
            {
                new UnFavoriteNoteUseCase(new UnFavoriteRequest
                {
                    UserId = UserManager.CurrentUser.UserId,
                    NoteId = Note.NoteId,
                }, new UnFavoriteNotePresenterCallback(this)).Execute();
            }
            else
            {
                new FavoriteNoteUseCase(new FavoriteNoteRequest
                {
                    NoteId = Note.NoteId,
                    UserId = UserManager.CurrentUser.UserId,
                }, new FavoriteNotePresenterCallback(this)).Execute();
            }
        }

        

        

        public void UpdateNote(string noteContent)
        {
            new UpdateNoteUseCase(new UpdateNoteRequest
            {
                NoteId = Note.NoteId,
                NoteTitle = Title,
                NoteContent = noteContent,
                NoteBg = NoteColor,
            }, new UpdateNotePresenterCallback(this)).Execute();
        }





        public class UpdateNotePresenterCallback : IPresenterCallback<UpdateNoteResponse>
        {
            private NoteDisplayUserControlViewModel _viewModel;
            public UpdateNotePresenterCallback(NoteDisplayUserControlViewModel viewModel)
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

            public async void OnSuccess(UpdateNoteResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    
                    _viewModel.OnNoteUpdated(result.Note);

                });
            }
        }

        public class UnFavoriteNotePresenterCallback : IPresenterCallback<UnFavoriteNoteResponse>
        {
            private NoteDisplayUserControlViewModel _viewModel;

            public UnFavoriteNotePresenterCallback(NoteDisplayUserControlViewModel viewModel)
            {
                _viewModel = viewModel;
            }
            public async void OnError(Exception error)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 async () =>
                 {
                     await new MessageDialog(error.Message).ShowAsync();

                 });
            }

            public async void OnSuccess(UnFavoriteNoteResponse response)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                  () =>
                  {
                      _viewModel.NoteUnFavorited(response.NoteId);
                  });
            }
        }

        private class FavoriteNotePresenterCallback : IPresenterCallback<FavoriteNoteResponse>
        {
            private NoteDisplayUserControlViewModel _viewModel;

            public FavoriteNotePresenterCallback(NoteDisplayUserControlViewModel viewModel)
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

            public async void OnSuccess(FavoriteNoteResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     _viewModel.NoteFavorited(result.NoteId);
                 });
            }
        }
    }


}
