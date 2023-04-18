using Notes_Application.Presenter.ViewModel.Models;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Notifications;

namespace Notes_Application.ViewModel.Models
{
    public class NotesVobj:NoteBobj,INotifyPropertyChanged
    {
        public override string Title
        {
            get => base.Title;
            set
            {
                if(base.Title != value)
                {
                    base.Title = value;
                    OnPropertyChanged();
                }
                
            }
        }

        public override string NoteColor
        {
            get => base.NoteColor;
            set
            {
                if(base.NoteColor != value)
                {
                    base.NoteColor = value;
                    OnPropertyChanged();
                }
            }
        }

        public override bool IsFavorite
        {
            get => base.IsFavorite;
            set
            {
                if(base.IsFavorite != value)
                {
                    base.IsFavorite = value;
                    OnPropertyChanged();
                }
            }
        }

        public override DateTime UpdatedAt
        {
            get => base.UpdatedAt;
            set
            {
                if(base.UpdatedAt != value )
                {
                    base.UpdatedAt = value;
                    OnPropertyChanged();
                }
            }
        }

        public NotesVobj(NoteBobj note):base(note)
        {
            this.NoteTags = new List<Tag>();
            foreach(var tag in note.NoteTags)
            {
                this.NoteTags.Add(new TagVobj(tag));
            }
           
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
            
    }

    
}
