using Microsoft.Toolkit.Uwp.UI;
using Notes_Application.Utility_Classes;
using Notes_Application.View;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Notes_Application.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

       
        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
            }
        }
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(Email)));
            }
        }
        private string _noteColor;
        public string NoteColor
        {
            get => _noteColor;
            set
            {
                if(value!= _noteColor)
                {
                    _noteColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoteColor)));
                }
                
            }
        }

        private List<Tag> _allTags;

        public List<Tag> AllTags
        {
            get => _allTags;
            set
            {
                _allTags = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllTags)));
            }
        }

        private ObservableCollection<Tag> _tagsCollectionSuggestion;

        public ObservableCollection<Tag> TagsCollectionSuggestion
        {
            get => _tagsCollectionSuggestion;
            set
            {
                _tagsCollectionSuggestion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TagsCollectionSuggestion)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            UserName = UserManager.CurrentUser.UserName;
            Email = UserManager.CurrentUser.Email;
            NoteColor = UserManager.CurrentUser.DefaultNoteColor;
            GetAllTags();
            TagsCollectionSuggestion = new ObservableCollection<Tag>();
            
        }
        
      
        public void OnTagsReceived(GetAllTagsResponse result)
        {
            AllTags = result.Tags;
            TagsCollectionSuggestion = new ObservableCollection<Tag>(AllTags);
        }
        public void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var input = ((TextBox)sender).Text;
            TagsCollectionSuggestion = new ObservableCollection<Tag>(AllTags.Where(t => t.Name.StartsWith(input, StringComparison.OrdinalIgnoreCase)).ToList());

        }
        public void GetAllTags()
        {
            new GetAllTagsUseCase(new GetAllTagsRequest
            {
                UserId = UserManager.CurrentUser.UserId,
            }, new GetAllTagsPresenterCallback(this)).Execute();
        }
        public void UpdateDefaultNoteColor(string noteColor)
        {
            new UpdateUserUseCase(new UpdateUserRequest
            {
                UserId = UserManager.CurrentUser.UserId,
                UserName = UserManager.CurrentUser.UserName,
                Password = UserManager.CurrentUser.Password,
                DefaultNoteColor= noteColor,

            }, new UpdateUserPresenterCallback(this)).Execute();
        }

        

        private class GetAllTagsPresenterCallback : IPresenterCallback<GetAllTagsResponse>
        {
            private MainPageViewModel _viewModel;

            public GetAllTagsPresenterCallback(MainPageViewModel viewModel)
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

            public async void OnSuccess(GetAllTagsResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     _viewModel.OnTagsReceived(result);
                 });
            }
        }

        public class UpdateUserPresenterCallback:IPresenterCallback<UpdateUserResponse>
        {
            private MainPageViewModel _viewModel;

            public UpdateUserPresenterCallback(MainPageViewModel viewModel)
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

            public async void OnSuccess(UpdateUserResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     UserManager.CurrentUser = result.UpdatedUser;
                 });
            }
        }
    }
    



}
