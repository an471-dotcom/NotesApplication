using Notes_Application.Presenter.ViewModel.Models;
using Notes_Application.Utility_Classes;
using Notes_Application.ViewModel.Models;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Domain.UseCase.Note;
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
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using static SQLite.SQLite3;

namespace Notes_Application.ViewModel
{
    public class TagsUserControlViewModel : INotifyPropertyChanged
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

        private List<Tag> _tagsSuggestion;
        public List<Tag> TagsSuggestion
        {
            get => _tagsSuggestion;
            set
            {
                _tagsSuggestion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TagsSuggestion)));
            }
        }

        private ObservableCollection<Tag> _noteTags;
        public ObservableCollection<Tag> NoteTags
        {
            get => _noteTags;
            set
            {
                if (value != _noteTags)
                {
                    _noteTags = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoteTags)));
                }
            }
        }

        private Brush _tagColor;

        public TagsUserControlViewModel()
        {
           
        }

        public Brush TagColor
        {
            get => _tagColor;
            set
            {
                if(value != _tagColor)
                {
                    _tagColor = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(TagColor)));
                }
            }
        }

        public Tag DeletedTag { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void SetNoteData(NotesVobj note)
        {
            Note = note;

            if (Note != null)
            {
                NoteTags = new ObservableCollection<Tag>(Note.NoteTags);
                TagColor = new SolidColorBrush(UtilityClass.GetColorFromHex(_note.NoteColor));
            }
        }

        public void OnNoteTagDeleted(int TagId)
        {
            Note.NoteTags.Remove(Note.NoteTags.FirstOrDefault(t => t.Id == TagId));
        }

        public void OnTagSuggestionReceived(GetTagsSuggestionResponse response)
        {
            List<Tag> suggestions = new List<Tag>();
            foreach(var tag in response.Tags)
            {
                if(NoteTags.FirstOrDefault(t => t.Id == tag.Id) == null)
                {
                    suggestions.Add(tag);
                }
                
            }
            TagsSuggestion = suggestions; 
        }

        public void OnTagAdded(CreateNoteTagResponse response)
        {
            Note.NoteTags.Add(new TagVobj(response.Tag));
            NoteTags.Add(new TagVobj(response.Tag));
        }

        public void AddRemovedTag()
        {
            if (DeletedTag != null)
            {
                Note.NoteTags.Add(DeletedTag);
                NoteTags.Remove(DeletedTag);
            }

        }

        public void OnTagDeleted(int TagId)
        {
            TagsSuggestion.Remove(TagsSuggestion.FirstOrDefault(t => t.Id== TagId));
        }

        public void OnTagUpdated(Tag updatedTag)
        {
            var tag = NoteTags.FirstOrDefault(t => t.Id == updatedTag.Id);
            tag.Name = updatedTag.Name;
            tag.TagColor= updatedTag.TagColor;
        }
        public void AddTag(string tagName)
        {
            new CreateNoteTagUseCase(new CreateNoteTagRequest
            {
                TagName = tagName,
                UserId = UserManager.CurrentUser.UserId,
                NoteId = Note.NoteId,
                TagColor=Colors.LightGray.ToString(),
           
                
            }, new CreateNoteTagPresenterCallback(this)).Execute();

        }

        public void GetTagsSuggestion(string input)
        {
            new GetTagsSuggestionUseCase(new GetTagsSuggestionRequest
            {
                input = input,
                UserId = UserManager.CurrentUser.UserId,
            }, new GetTagSuggestionPresenterCallback(this)).Execute();
        }

        public void UpdateTag(Tag tag)
        {
            new UpdateTagUseCase(new UpdateTagRequest
            {
                TagId = tag.Id,
                TagName = tag.Name,
                TagColor = tag.TagColor,

            },new UpdateTagPresenterCallback(this)).Execute();
        }

        public void DeleteNoteTag(int tagId)
        {
            new DeleteNoteTagUseCase(new DeleteNoteTagRequest
            {
                TagId = tagId,
                NoteId = Note.NoteId,

            }, new DeleteNoteTagPresenterCallback(this)).Execute();
        }

        public void DeleteTag(int tagId)
        {
            new DeleteTagUseCase(new DeleteTagResquest
            {
                TagId = tagId,

            }, new DeleteTagPresenterCallback(this)).Execute();
        }
        private class CreateNoteTagPresenterCallback : IPresenterCallback<CreateNoteTagResponse>
        {
            private TagsUserControlViewModel _viewModel;
            public CreateNoteTagPresenterCallback(TagsUserControlViewModel viewModel)
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


        private class GetTagSuggestionPresenterCallback : IPresenterCallback<GetTagsSuggestionResponse>
        {
            private TagsUserControlViewModel _viewModel;
            public GetTagSuggestionPresenterCallback(TagsUserControlViewModel viewModel)
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

        public class DeleteNoteTagPresenterCallback : IPresenterCallback<DeleteNoteTagResponse>
        {
            private TagsUserControlViewModel _viewModel;
            public DeleteNoteTagPresenterCallback(TagsUserControlViewModel viewModel)
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

            public async void OnSuccess(DeleteNoteTagResponse result) 
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     _viewModel.OnNoteTagDeleted(result.TagId);
                 });
            }
        }

        private class DeleteTagPresenterCallback:IPresenterCallback<DeleteTagResponse>
        {
            private TagsUserControlViewModel _viewModel;

            public DeleteTagPresenterCallback(TagsUserControlViewModel viewModel)
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

            public  async void OnSuccess(DeleteTagResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                  () =>
                  {
                      _viewModel.OnTagDeleted(result.DeletedTagId);
                  });
            }
        }

        private class UpdateTagPresenterCallback:IPresenterCallback<UpdateTagResponse>
        {
            private readonly TagsUserControlViewModel _viewModel;

            public UpdateTagPresenterCallback(TagsUserControlViewModel viewModel)
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

            public async void OnSuccess(UpdateTagResponse response)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _viewModel.OnTagUpdated(response.UpdatedTag);
                });
            }
        }
    }


}
