using Notes_Library.Model.BussinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Application.Presenter.ViewModel.Models
{
    public class NoteBookVobj : NoteBookBobj, INotifyPropertyChanged
    {
        public override string NoteBookCover
        {
            get => base.NoteBookCover;
            set
            {
               if( base.NoteBookCover != value)
                {
                    base.NoteBookCover = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(NoteBookCover)));
                }


            }
        }

        public NoteBookVobj(NoteBookBobj noteBook):base(noteBook) 
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
