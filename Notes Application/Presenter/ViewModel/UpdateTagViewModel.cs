using Notes_Application.ViewModel;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase.Note;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;

namespace Notes_Application.Presenter.ViewModel
{
    public class UpdateTagViewModel:INotifyPropertyChanged
    {
        private Tag _CurrentTag;
        public Tag CurrentTag
        {
            get => _CurrentTag; 
            set 
            { 
                if(value!= _CurrentTag)
                {
                    _CurrentTag = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(CurrentTag)));
                }
                
            }
        }

        private string _tagName;
        public string TagName
        {
            get => _tagName; 
            set
            {
                if(value != _tagName)
                {
                    _tagName = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(TagName)));
                }
            }
        }

        private string _tagColor = "#FFF2F2F2";
        public string TagColor
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

        public List<SolidColorBrush> colors;

        public event PropertyChangedEventHandler PropertyChanged;


        public UpdateTagViewModel()
        {
            colors = new List<SolidColorBrush>()
            {
               new SolidColorBrush(Colors.LightCoral),
               new SolidColorBrush( Colors.SkyBlue),
                new SolidColorBrush(Colors.LimeGreen),
            };
        }
        public void SetTagData(Tag tag)
        {
            if(tag!= null)
            {
                _CurrentTag = tag;
                TagName = tag.Name;
                TagColor = tag.TagColor;
            }
        }

        public void UpdateTag()
        {
            new UpdateTagUseCase(new UpdateTagRequest
            {
                TagId = CurrentTag.Id,
                TagName = TagName,
                TagColor = TagColor

            }, new UpdateTagPresenterCallback(this)).Execute();
        }

        public void OnTagUpdated(Tag updatedTag)
        {
            CurrentTag.Name = updatedTag.Name;
            CurrentTag.TagColor = updatedTag.TagColor;
        }
        private class UpdateTagPresenterCallback : IPresenterCallback<UpdateTagResponse>
        {
            private readonly UpdateTagViewModel _viewModel;

            public UpdateTagPresenterCallback(UpdateTagViewModel viewModel)
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
