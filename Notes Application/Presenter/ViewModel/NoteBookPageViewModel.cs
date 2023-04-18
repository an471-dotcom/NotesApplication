using Notes_Application.Presenter.ViewModel.Models;
using Notes_Application.Utility_Classes;
using Notes_Application.ViewModel.Models;
using Notes_Library.DataManager;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Domain.UseCase.Notebook;
using Notes_Library.Model;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using static SQLite.SQLite3;

namespace Notes_Application.ViewModel
{

    public  class NoteBookPageViewModel:INotifyPropertyChanged                             
    {
        public ObservableCollection<NoteBookVobj> Notebooks;


       

        private List<CoverImage> _coverImages;

        public List<CoverImage> CoverImages
        {
            get => _coverImages;
            set
            {
                if(value != _coverImages)
                {
                    _coverImages = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(CoverImages)));
                }
            }
        }

        private CoverImage _selectedcover;
        public CoverImage SelectedCover
        {
            get => _selectedcover;
            set
            {
                if(value != _selectedcover)
                {
                    _selectedcover = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(SelectedCover)));
                }
            }
        }
        private NoteBookVobj _selectedNotebook;
       
        public NoteBookPageViewModel()
        {
            Notebooks = new ObservableCollection<NoteBookVobj>();
            
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public async void GetNoteBookCoverImages(NoteBookVobj notebook)
        {
            CoverImages = await new ImageRepository().GetImages();
            SelectedCover = CoverImages.FirstOrDefault(c => c.Name == notebook.NoteBookCover);
            _selectedNotebook = notebook;
        }

        public void GetNotebooks()
        {
            new GetNoteBooksUseCase(new GetNotebooksRequest
            {
                UserId = UserManager.CurrentUser.UserId,
            }, new GetNotebooksPresenterCallback(this)).Execute();

        }
        public async void CreateNotebookButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateNoteBookUseCase(new CreateNotebookRequest
            {
                Title = "Untitled",
                NoteBookCover = (await new ImageRepository().GetImages())[1].Name,
                UserId = UserManager.CurrentUser.UserId,
            }, new CreateNotebookPresenterCallback(this)).Execute();

        }

        public void DeleteNotebook(int notebookId)
        {
            new DeleteNoteBookUseCase(new DeleteNotebookRequest
            {
                NotebookId = notebookId,

            }, new DeleteNotebookPresenterCallback(this)).Execute();
        }

        public void ChangeNotebookCover(string notebookCover)
        {
            new ChangeNoteBookCover(new ChangeNoteBookCoverRequest
            {
                NoteBookId = _selectedNotebook.NotebookId,
                NoteBookCover = notebookCover

            },new ChangeNoteBookCoverPresenterCallback(this)).Execute();
        }

        public void AddToNotebooksCollection(NoteBookVobj notebook)
        {
            Notebooks.Insert(0, notebook);
        }
        
        
        public void PopulateCollection(List<NoteBookVobj> notebooks)
        {
           
            Notebooks.Clear();
            notebooks.ForEach(notebook =>
            {
                Notebooks.Insert(0, notebook);
            });
        }
        

        public void OnNotebookDeleted(int notebookId)
        {
            var notebook = Notebooks.FirstOrDefault(nb => nb.NotebookId == notebookId);
            Notebooks.Remove(notebook);
        }
       
        public void OnNoteBookCoverUpdated(string NoteBookCover)
        {
            _selectedNotebook.NoteBookCover = NoteBookCover;
        }
       

        public void OnNotebookUpdated(NoteBookVobj notebook)
        {
            var currentNotebook = Notebooks.FirstOrDefault(nb => nb.NotebookId == notebook.NotebookId);
            currentNotebook.Name = notebook.Name;
            currentNotebook.ModifiedAt = notebook.ModifiedAt;
        }

        public class ImageRepository
        {
            private List<CoverImage> Images = new List<CoverImage>();


            public async Task<List<CoverImage>> GetImages()
            {
                string root = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
                string path = root + @"\Assets\NotebookCovers\";
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
                var images = await folder.GetFilesAsync();
                foreach (var image in images)
                {
                    Images.Add(new CoverImage { Name = image.Name ,ImagePath=image.Path});
                }

                return Images;
            }
        }

       

        private class CreateNotebookPresenterCallback : IPresenterCallback<CreateNotebookResponse>
        {
            private NoteBookPageViewModel _viewModel;

            public CreateNotebookPresenterCallback(NoteBookPageViewModel viewModel)
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

            public async void OnSuccess(CreateNotebookResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     _viewModel.AddToNotebooksCollection(new NoteBookVobj(result.Notebook));
                 });
            }
        }

        private class GetNotebooksPresenterCallback : IPresenterCallback<GetNoteBooksResponse>
        {
            private NoteBookPageViewModel _viewModel;

            public GetNotebooksPresenterCallback(NoteBookPageViewModel viewModel)
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

            public async void OnSuccess(GetNoteBooksResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     List<NoteBookVobj> notebooks = new List<NoteBookVobj>();
                     foreach(var notebook in result.Notebooks)
                     {
                         notebooks.Add(new NoteBookVobj(notebook));
                     }

                     _viewModel.PopulateCollection(notebooks);
                 });
            }
        }

        private class DeleteNotebookPresenterCallback : IPresenterCallback<DeleteNoteBookResponse>
        {
            private NoteBookPageViewModel _viewModel;

            public DeleteNotebookPresenterCallback(NoteBookPageViewModel viewModel)
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

            public async void OnSuccess(DeleteNoteBookResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     _viewModel.OnNotebookDeleted(result.NotebookId);
                 });
            }
        }
        private class ChangeNoteBookCoverPresenterCallback : IPresenterCallback<ChangeNoteBookCoverResponse>
        {
            private NoteBookPageViewModel _viewModel;

            public ChangeNoteBookCoverPresenterCallback(NoteBookPageViewModel viewModel)
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

            public async void OnSuccess(ChangeNoteBookCoverResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _viewModel.OnNoteBookCoverUpdated(result.NoteBookCover);
                });
            }
        }


    }
    
}
