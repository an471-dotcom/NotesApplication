using Notes_Application.Presenter.ViewModel.Models;
using Notes_Application.UserControls;
using Notes_Application.Utility_Classes;
using Notes_Application.ViewModel.Models;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Printers;
using Windows.Media.DialProtocol;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;


namespace Notes_Application.ViewModel
{
    public class NoteBookUserControlViewModel:INotifyPropertyChanged
    {
       
        private string _notebookName;

        public string NotebookName
        {
            get => _notebookName;
            set
            {
                _notebookName = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(NotebookName)));
                
            }
        }
        private NoteBookVobj _notebook;

        public NoteBookVobj Notebook
        {
            get => _notebook;
            set
            {
                if(value != _notebook)
                {
                    _notebook = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Notebook)));
                }
            }
        }

        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly= value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(IsReadOnly)));
            }
        }

        private string _noteCountText;
        public string NoteCountText
        {
            get => _noteCountText;
            set
            {
                if(value != _noteCountText)
                {
                    _noteCountText = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(NoteCountText)));
                }
            }
        }

        private string _noteBookCover = "/Assets/Cover.jpg";
        public string NoteBookCover
        {
            get => _noteBookCover;
            set
            {
                if(value != _noteBookCover)
                {
                    _noteBookCover = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoteBookCover)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public NoteBookUserControlViewModel()
        {
           
           
        }

        public void ResetNotebookName()
        {
            NotebookName = UtilityClass.TruncateString(Notebook?.Name, 10);
            IsReadOnly = true;

           
        }
        public void SetNotebookData(NoteBookVobj notebook)
        {

            
            Notebook = notebook;
            NotebookName = UtilityClass.TruncateString(Notebook?.Name,10);
            IsReadOnly = true;
            if(Notebook!= null)
            {
                NoteCountText = $"{Notebook.NoteCount} Notes";
                NoteBookCover = $"/Assets/NotebookCovers/{Notebook.NoteBookCover}";
            }
            
        }

        public void SetNoteBookName()
        {
            IsReadOnly= false;
            NotebookName = Notebook.Name;
            
        }

        public void RenameNotebook()
        {
            new RenameNoteBookUseCase(new RenameNoteBookRequest
            {
                NoteBookId = Notebook.NotebookId,
                Title = NotebookName,
            }, new RenameNotebookPresenterCallback(this)).Execute();
        }
        
        public void OnNotebookUpdated(NoteBookVobj udpatedNotebook)
        {
            SetNotebookData(udpatedNotebook);
        }
    }

    public class RenameNotebookPresenterCallback : IPresenterCallback<RenameNoteBookResponse>
    {
        private NoteBookUserControlViewModel _viewModel;

        public RenameNotebookPresenterCallback(NoteBookUserControlViewModel viewModel)
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

        public async void OnSuccess(RenameNoteBookResponse result)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
             () =>
             {  
                 
                 _viewModel.OnNotebookUpdated(new NoteBookVobj(result.UpdatedNotebook));
             });
        }
    }
    

   
    
}
