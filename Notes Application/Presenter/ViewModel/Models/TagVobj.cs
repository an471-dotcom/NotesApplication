using Notes_Library.Models;
using Notes_Library.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Notes_Application.Presenter.ViewModel.Models
{
    public class TagVobj : Tag, INotifyPropertyChanged
    {
        public override string Name
        {
            get => base.Name;
            set
            {
                if (base.Name != value)
                {
                    base.Name = value;
                    OnPropertyChanged(nameof(Name));
                }

            }
        }

        public override string TagColor
        {
            get => base.TagColor;
            set
            {
                if (base.TagColor != value)
                {
                    base.TagColor = value;
                    OnPropertyChanged(nameof(TagColor));
                }
               
            }
        }
        public TagVobj()
        {

        }
        public TagVobj(Tag tag):base(tag)
        {
            TagNotificationcs.TagUpdated += TagNotificationcs_TagUpdated;
        }

        private void TagNotificationcs_TagUpdated(Tag obj)
        {
            if(obj?.Id != Id)
            {
                return;
            }
            Name= obj.Name;
            TagColor= obj.TagColor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {

                   PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


               });
            
        }
    }
}
