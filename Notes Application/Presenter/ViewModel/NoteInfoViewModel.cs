using Microsoft.Toolkit.Uwp.UI;
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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Notes_Application.ViewModel
{
    public class NoteInfoViewModel:INotifyPropertyChanged
    {
       
        private NotesVobj _noteData;
        public NotesVobj NoteData
        {
            get => _noteData;
            set
            {
                _noteData  = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoteData)));
            }

        }

        public  Tag DeletedTag { get; set; }

        private List<Tag> _tagsSuggestion;

        public List<Tag> TagsList
        {
            get => _tagsSuggestion;
            set
            {
                _tagsSuggestion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TagsList)));
            }
        }


        public void SetNoteData(NotesVobj note)
        {
            NoteData = note;
        }

     
        public NoteInfoViewModel() 
        { 
            
            TagsList = new List<Tag>();
            
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

        public void AddTag(string tagName,int noteId)
        {  
            new CreateNoteTagUseCase(new CreateNoteTagRequest
            {
                TagName = tagName,
                UserId = UserManager.CurrentUser.UserId,
                NoteId = noteId,
                TagColor = "#FFF2F2F2" 

            }, new CreateNoteTagPresenterCallback(this)).Execute();
              
        }

        public void DeleteNoteTag(int noteId,int tagId)
        {
            new DeleteNoteTagUseCase(new DeleteNoteTagRequest
            {
                TagId = tagId,
                NoteId = noteId,
            }, new DeleteNoteTagPresenterCallback(this)).Execute();
        }

      

        public void GetTagsSuggestion(string input)
        {
            new GetTagsSuggestionUseCase(new GetTagsSuggestionRequest
            {
                input = input,
                UserId = UserManager.CurrentUser.UserId,
            }, new GetTagSuggestionPresenterCallback(this)).Execute();
        }

        public void OnTagAdded(CreateNoteTagResponse response)
        {
            NoteData.NoteTags.Add(response.Tag);
           
        }


        public void OnTagSuggestionReceived(GetTagsSuggestionResponse response)
        {
            TagsList = response.Tags;
        }
        public void AddRemovedTag()
        {
            if(DeletedTag!= null)
            {
                NoteData.NoteTags.Add(DeletedTag);
            }

        }

        private class GetTagSuggestionPresenterCallback : IPresenterCallback<GetTagsSuggestionResponse>
        {
            private NoteInfoViewModel _viewModel;
            public GetTagSuggestionPresenterCallback(NoteInfoViewModel viewModel)
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

            public async void OnSuccess(GetTagsSuggestionResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     _viewModel.OnTagSuggestionReceived(result);
                 });
            }
        }

    }

    public class CreateNoteTagPresenterCallback : IPresenterCallback<CreateNoteTagResponse>
    {
        NoteInfoViewModel _viewModel;
        public CreateNoteTagPresenterCallback(NoteInfoViewModel viewModel)
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

        public async void OnSuccess(CreateNoteTagResponse result)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
             () =>
             {
                 _viewModel.OnTagAdded(result);
             });
        }
    }
    public class DeleteNoteTagPresenterCallback : IPresenterCallback<DeleteNoteTagResponse>
    {
        private NoteInfoViewModel _viewModel;
        public DeleteNoteTagPresenterCallback(NoteInfoViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public async void OnError(Exception e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
             async () =>
             {
                
                 await new MessageDialog(e.Message).ShowAsync();
                 _viewModel.AddRemovedTag();

             });
        }

        public void OnSuccess(DeleteNoteTagResponse result){}
    }

    

    
}
