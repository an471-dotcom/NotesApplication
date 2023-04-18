using Notes_Application.Utility_Classes;
using Notes_Application.View;
using Notes_Library.DataManager;
using Notes_Library.Domain.UseCase;
using Notes_Library.Domain;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using System.Threading;
using Notes_Application.UserControls;
using System.Runtime.InteropServices;
using Windows.UI.Xaml.Data;
using System.Xml.Linq;
using System.Reflection.Metadata;
using Windows.Devices.Bluetooth.Advertisement;
using Notes_Library.Model.BussinessObjects;
using Microsoft.Toolkit.Uwp.UI;
using static SQLite.SQLite3;
using Notes_Application.ViewModel.Models;
using System.Collections.Specialized;
using ColorCode.Common;
using Microsoft.Toolkit.Collections;
using Windows.UI.Xaml.Documents;
using static Notes_Application.ViewModel.NoteDisplayUserControlViewModel;
using System.Reflection;
using Windows.Networking.NetworkOperators;
using Notes_Library.Domain.UseCase.Note;
using static Notes_Application.ViewModel.ShareInfoUserControlViewModel;
using Windows.ApplicationModel.Resources;

namespace Notes_Application.ViewModel
{
    public class NotesPageViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<NotesVobj> _notes;

        private ObservableCollection<NotesVobj> _filteredNotes;

        public ObservableCollection<NotesVobj> FilteredNotes
        {
            get { return _filteredNotes; }
            set
            {
                _filteredNotes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilteredNotes)));
            }
        }


        private string _titleText;
        public string TitleText
        {
            get => _titleText;
            set
            {
                _titleText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitleText)));
            }
        }

        private Visibility _backbuttonVisibility;
        public Visibility BackButtonVisibility
        {
            get => _backbuttonVisibility;
            set
            {
                _backbuttonVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackButtonVisibility)));
            }
        }
        public NoteType NoteType { get; set; }

        public int NotebookId { get; set; } = 0;

        public List<Tag> _allTags;
        public List<Tag> AllTags
        {
            get => _allTags;
            set
            {
                _allTags = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllTags)));
            }
        }

        private ObservableCollection<Tag> _tagsSuggestion;
        public ObservableCollection<Tag> TagsSuggestion
        {
            get=> _tagsSuggestion;
            set
            {
                if(value != _tagsSuggestion)
                {
                    _tagsSuggestion = value;
                    PropertyChanged.Invoke(this,new PropertyChangedEventArgs(nameof(TagsSuggestion)));
                }
            }
        }

        private NotesVobj _selectedNote = null;
        public NotesVobj SelectedNote
        {
            get => _selectedNote;
            set
            {
                if (value != SelectedNote)
                {
                    _selectedNote = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNote)));
                }

            }
        }

        private List<string> _sortComboBoxItems;
        public List<string> SortComboBoxItems
        {
            get => _sortComboBoxItems;
            set
            {
                if (value != _sortComboBoxItems)
                {
                    _sortComboBoxItems = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SortComboBoxItems)));
                }
            }

        }

        private string _selectedSortOption;
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (value != _selectedSortOption)
                {
                    _selectedSortOption = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSortOption)));
                }

            }
        }

        private Visibility _createNoteButtonVisibility;
        public Visibility CreateNoteButtonVisibility
        {
            get => _createNoteButtonVisibility;
            set
            {
                if (value != _createNoteButtonVisibility)
                {
                    _createNoteButtonVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreateNoteButtonVisibility)));
                }
            }
        }

        private List<string> _sortOptions = new List<string> { "Title", "Date Created", "Date Modified" };

        private bool _isCollectionEmpty = false;
        public bool IsCollectionEmpty
        {
            get => _isCollectionEmpty;
            set
            {

                _isCollectionEmpty = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCollectionEmpty)));


            }

        }

        

        private Visibility _clearSelectionVisibility = Visibility.Collapsed;
        public Visibility ClearSelectionVisibility
        {
            get => _clearSelectionVisibility;
            set
            {
                if (value != _clearSelectionVisibility)
                {
                    _clearSelectionVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ClearSelectionVisibility)));
                }
            }
        }


        ResourceLoader resourceLoader;


        public NotesPageViewModel()
        {
            AllTags = new List<Tag>();
            
            _notes = new ObservableCollection<NotesVobj>();
            _notes.CollectionChanged += Notes_CollectionChanged;
            SortComboBoxItems = _sortOptions;
            SelectedSortOption = "Date Created";
            FilteredNotes = new ObservableCollection<NotesVobj>();
            resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

        }


        private void Notes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var note = item as NotesVobj;
                    var noteTobeDeleted = FilteredNotes.FirstOrDefault(n => n.NoteId == note.NoteId);

                    if (noteTobeDeleted == SelectedNote)
                    {
                        int index = FilteredNotes.IndexOf(noteTobeDeleted);
                        FilteredNotes.RemoveAt(index);
                        if (FilteredNotes.Count() > 0)
                        {
                            if (index == FilteredNotes.Count())
                            {
                                SelectedNote = FilteredNotes[index - 1];
                            }
                            else
                            {
                                SelectedNote = FilteredNotes[index];
                            }
                        }
                        else
                        {
                            IsCollectionEmpty = true;
                        }
                    }
                    else
                    {
                        FilteredNotes.Remove(noteTobeDeleted);
                    }

                }

            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var note = item as NotesVobj;
                    FilteredNotes.Insert(0, note);
                    SelectedNote = FilteredNotes[0];
                    IsCollectionEmpty = false;

                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.Parameter is NoteType)
            {
                NoteType = (NoteType)e.Parameter;
                BackButtonVisibility = Visibility.Collapsed;
                if (NoteType == NoteType.AllNotes)
                {
                    CreateNoteButtonVisibility = Visibility.Visible;
                }
                else
                {
                    CreateNoteButtonVisibility = Visibility.Collapsed;
                }

                new GetAllNotesUseCase(new GetAllNotesRequest
                {
                    UserId = UserManager.CurrentUser.UserId,

                }, new GetNotesPresenterCallback(this)).Execute();

            }
            else if (e.Parameter is int)
            {
                NoteType = NoteType.NotebookNotes;
                BackButtonVisibility = Visibility.Visible;
                CreateNoteButtonVisibility = Visibility.Visible;
                NotebookId = (int)e.Parameter;
                new GetNoteBookNotesUseCase(new GetNotebookNotesRequest
                {
                    NotebookId = NotebookId,
                }, new GetNotesPresenterCallback(this)).Execute();

            }
         
        }
        private void PopulateCollection(List<NoteBobj> notes)
        {

            notes = notes.OrderBy(s => s.CreatedAt).ToList();
            foreach (var note in notes)
            {
                _notes.Add(new NotesVobj(note));
            }

            if (FilteredNotes.Count > 0)
            {
                SetSelectedNote();
                IsCollectionEmpty = false;
            }
            else
            {
                SelectedNote = null;
                IsCollectionEmpty = true;
            }
        }



        public void ClearTagsSelection()
        {
            GetAllNoteTags();
            SetFilterdNotes();
            
        }


        public void SetFilterdNotes()
        {
           
            FilteredNotes = new ObservableCollection<NotesVobj>(_notes.OrderByDescending(note => note.CreatedAt));
            ClearSelectionVisibility = Visibility.Collapsed;
            CreateNoteButtonVisibility = Visibility.Visible;
            if (FilteredNotes.Count() > 0)
            {
                SelectedNote = FilteredNotes[0];
                TitleText = UtilityClass.GetDescriptionFromEnum(NoteType);
            }
            
        }


        public void SetSelectedNote()
        {

            if (FilteredNotes.Count > 0)
            {
                SelectedNote = FilteredNotes[0];

            }
        }




        public void SetListViewSelectedNote()
        {
            if (FilteredNotes.Count() > 0)
            {
                SelectedNote = FilteredNotes[0];
            }
        }

        public void SortNotes()
        {
            switch (SelectedSortOption)
            {
                case "Title":
                    {
                        List<NotesVobj> sortedNotes = FilteredNotes.OrderBy(note => note.Title).ToList();
                        FilteredNotes.Clear();
                        foreach (var note in sortedNotes)
                        {
                            FilteredNotes.Add(note);
                        }
                    }

                    break;
                case "Date Created":
                    {
                        List<NotesVobj> sortedNotes = FilteredNotes.OrderByDescending(note => note.CreatedAt).ToList();
                        FilteredNotes.Clear();
                        foreach (var note in sortedNotes)
                        {
                            FilteredNotes.Add(note);
                        }

                    }
 
                    break;
                case "Date Modified":
                    {
                        List<NotesVobj> sortedNotes = FilteredNotes.OrderByDescending(note => note.UpdatedAt).ToList();
                        FilteredNotes.Clear();
                        foreach (var note in sortedNotes)
                        {
                            FilteredNotes.Add(note);
                        }

                    }
                    break;
            }
            SetListViewSelectedNote();
        }


        public void FilterByTag(Tag tag)
        {
            TitleText = $"#{UtilityClass.TruncateString(tag.Name, 20)}";
            List<NotesVobj> notes = new List<NotesVobj>();
            foreach (var note in _notes)
            {
                foreach (var noteTag in note.NoteTags)
                {
                    if (noteTag.Id == tag.Id)
                    {
                        notes.Add(note);
                        break;
                    }
                }
            }

            FilteredNotes = new ObservableCollection<NotesVobj>(notes);
            SortNotes();
            ClearSelectionVisibility = Visibility.Visible;
            CreateNoteButtonVisibility = Visibility.Collapsed;
            if (FilteredNotes.Count() > 0)
            {
                SelectedNote = FilteredNotes[0];
            }
        }
        public void GetAllNoteTags()
        {

            TagsSuggestion = new ObservableCollection<Tag>();


            foreach (var note in _notes.ToList())
            {
                foreach (var noteTag in note.NoteTags)
                {

                    var tag = TagsSuggestion.FirstOrDefault(t => t.Id == noteTag.Id);
                    if (tag == null)
                    {
                        TagsSuggestion.Add(noteTag);
                    }

                }
            }
            
        }



        public void OnNoteCreated(NotesVobj note)
        {
            _notes.Insert(0, note);

        }

        public void RemoveNoteFromFavorites(int noteId)
        {
            if (NoteType == NoteType.FavoriteNotes)
            {
                var note = _notes.FirstOrDefault(n => n.NoteId == noteId);
                if (note != null)
                {
                    _notes.Remove(note);
                }
            }

        }

        public void NoteDeleted(int noteId)
        {
            _notes.Remove(_notes.FirstOrDefault(n => n.NoteId == noteId));
        }

    
        public void NoteFavorited(int noteId)
        {
            _notes.FirstOrDefault(n => n.NoteId == noteId).IsFavorite = true;

        }
        public void NoteUnFavorited(int noteId)
        {
            _notes.FirstOrDefault(n => n.NoteId == noteId).IsFavorite = false;
            RemoveNoteFromFavorites(noteId);
        }

        public void OnNotesReceived(GetAllNotesResponse result)
        {
            List<NoteBobj> notes = new List<NoteBobj>();
            switch (NoteType)
            {
                case NoteType.AllNotes:

                    TitleText = resourceLoader.GetString("All Notes");
                    notes = result.Notes.Where(n => !n.IsSharedNote).ToList();
                    break;
                case NoteType.FavoriteNotes:
                    TitleText = resourceLoader.GetString("Favorites");
                    notes = result.Notes.Where(n => n.IsFavorite).ToList();
                    break;
                case NoteType.SharedNotes:
                    TitleText = resourceLoader.GetString("Shared Notes");
                    notes = result.Notes.Where(n => n.IsSharedNote).ToList();
                    break;

            }
            PopulateCollection(notes);
        }
        public void OnTagDeleted(int tagId)
        {


            foreach (var note in _notes)
            {
                foreach (var tag in note.NoteTags.ToList())
                {
                    if (tag.Id == tagId)
                    {

                        note.NoteTags.Remove(tag);
                    }

                }
            }


            foreach (var note in FilteredNotes)
            {
                foreach (var tag in note.NoteTags)
                {
                    if (tag.Id == tagId)
                    {
                        note.NoteTags.Remove(tag);
                    }

                }
            }
        }
        public void OnNotebookNotesReceived(GetNotebookNotesResponse result)
        {
            TitleText = UtilityClass.TruncateString(result.NotebookTitle, 30);
            PopulateCollection(result.Notes);
        }
        public void OnTaggedNotesReceived(GetNotesByTagResponse result)
        {
            TitleText = "#" + result.TagName;
            PopulateCollection(result.Notes);
        }

        public void OnNoteUpdated(NoteBobj note)
        {

            var updatedNote = note;
            var noteToUpdate = _notes.FirstOrDefault(n => n.NoteId == updatedNote.NoteId);
            var noteIndex = _notes.IndexOf(noteToUpdate);
            if (noteIndex != -1)
            {
                _notes[noteIndex].Title = updatedNote.Title;
                _notes[noteIndex].NoteContent = updatedNote.NoteContent;
                _notes[noteIndex].UpdatedAt = updatedNote.UpdatedAt;
                _notes[noteIndex].NoteColor = updatedNote.NoteColor;
            }
        }
        public void CreateNote()
        {
            new CreateNoteUseCase(new CreateNoteRequest
            {
                NoteTitle = "Title",
                CancellationToken = new CancellationTokenSource().Token,
                NoteBg = UserManager.CurrentUser.DefaultNoteColor,
                NoteContent = string.Empty,
                NotebookId = NotebookId,
                CreatedBy = UserManager.CurrentUser.UserId,
            }, new CreateNotePresenterCallback(this)).Execute();
        }
        public void DeleteNote(int noteId)
        {
            new DeleteNoteUseCase(new DeleteNoteRequest
            {
                UserId = UserManager.CurrentUser.UserId,
                NoteId = noteId,
            }, new DeleteNotePresenterCallback(this)).Execute();

        }
        public void ToggleFavoriteStatus(NotesVobj note)
        {
            if (note.IsFavorite)
            {
                new UnFavoriteNoteUseCase(new UnFavoriteRequest
                {
                    UserId = UserManager.CurrentUser.UserId,
                    NoteId = note.NoteId,

                }, new UnFavoriteNotePresenterCallback(this)).Execute();
            }
            else
            {
                new FavoriteNoteUseCase(new FavoriteNoteRequest
                {
                    NoteId = note.NoteId,
                    UserId = UserManager.CurrentUser.UserId,
                }, new FavoriteNotePresenterCallback(this)).Execute();
            }
        }
        public void UnshareNote(int noteId)
        {
            new UnshareNoteUseCase(new UnshareNoteRequest
            {
                NoteId = noteId,
                UserId = UserManager.CurrentUser.UserId,
            }, new UnshareNotePresenterCallback(this)).Execute();
        }
        public void DeleteTag(int tagId)
        {
            new DeleteTagUseCase(new DeleteTagResquest
            {
                TagId = tagId,

            }, new DeleteTagPresenterCallback(this)).Execute();
        }

        public class CreateNotePresenterCallback : IPresenterCallback<CreateNoteResponse>
        {
            private NotesPageViewModel _viewModel;
            public CreateNotePresenterCallback(NotesPageViewModel viewModel)
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

            public async void OnSuccess(CreateNoteResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    var noteVobj = new NotesVobj(result.Note);
                    _viewModel.OnNoteCreated(noteVobj);

                });
            }
        }

        public class UnFavoriteNotePresenterCallback : IPresenterCallback<UnFavoriteNoteResponse>
        {
            private NotesPageViewModel _viewModel;

            public UnFavoriteNotePresenterCallback(NotesPageViewModel viewModel)
            {
                _viewModel = viewModel;
            }
            public void OnError(Exception error)
            {
               _ =  CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 async () =>
                 {
                     await new MessageDialog(error.Message).ShowAsync();

                 });
            }

            public void OnSuccess(UnFavoriteNoteResponse response)
            {
                _ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     _viewModel.NoteUnFavorited(response.NoteId);
                 });
            }
        }

        private class DeleteNotePresenterCallback : IPresenterCallback<DeleteNoteResponse>
        {
            private NotesPageViewModel _viewModel;

            public DeleteNotePresenterCallback(NotesPageViewModel viewModel)
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

            public async void OnSuccess(DeleteNoteResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                  () =>
                  {
                      _viewModel.NoteDeleted(result.NoteId);
                  });
            }
        }

        private class FavoriteNotePresenterCallback : IPresenterCallback<FavoriteNoteResponse>
        {
            private NotesPageViewModel _viewModel;

            public FavoriteNotePresenterCallback(NotesPageViewModel viewModel)
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

        private class GetNotesPresenterCallback : IPresenterCallback<GetAllNotesResponse>, IPresenterCallback<GetNotebookNotesResponse>
        {
            private NotesPageViewModel _viewModel;

            public GetNotesPresenterCallback(NotesPageViewModel viewModel)
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


            public async void OnSuccess(GetAllNotesResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     _viewModel.OnNotesReceived(result);
                 });
            }

            public async void OnSuccess(GetNotebookNotesResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _viewModel.OnNotebookNotesReceived(result);
                });
            }
        }

        private class GetNotesByTagPresenterCallback : IPresenterCallback<GetNotesByTagResponse>
        {
            private NotesPageViewModel _viewModel;

            public GetNotesByTagPresenterCallback(NotesPageViewModel viewModel)
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

            public async void OnSuccess(GetNotesByTagResponse response)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {

                    _viewModel.OnTaggedNotesReceived(response);
                });
            }
        }

        private class DeleteTagPresenterCallback : IPresenterCallback<DeleteTagResponse>
        {
            private NotesPageViewModel _viewModel;

            public DeleteTagPresenterCallback(NotesPageViewModel viewModel)
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

            public async void OnSuccess(DeleteTagResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                  () =>
                  {
                      _viewModel.OnTagDeleted(result.DeletedTagId);
                  });
            }
        }

        public class UnshareNotePresenterCallback : IPresenterCallback<UnshareNoteResponse>
        {
            private NotesPageViewModel _viewModel;
            public UnshareNotePresenterCallback(NotesPageViewModel viewModel)
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
                    _viewModel.NoteDeleted(result.NoteId);

                });
            }
        }
    }




}
