using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Application.Presenter.ViewModel
{
    public class CommentsUserControlViewModel:INotifyPropertyChanged
    {
        private Comment _comment;
        public Comment Comment
        {
            get { return _comment; }
            set
            {
                if (value!= _comment)
                {
                    _comment= value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("Comment"));
                }
            }
        }
        public void SetData(Comment comment)
        {
            if(comment!= null)
            {
                Comment= comment;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
