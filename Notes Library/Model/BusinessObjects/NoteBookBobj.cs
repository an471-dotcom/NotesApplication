using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Model.BussinessObjects
{
    public class NoteBookBobj:NoteBook
    {
       
        public virtual int NoteCount { get; set; }

        public NoteBookBobj() { }


        public NoteBookBobj(NoteBookBobj notebookBobj)
        {
            NotebookId = notebookBobj.NotebookId;
            Name = notebookBobj.Name;
            CreatedBy = notebookBobj.CreatedBy;
            ModifiedAt = notebookBobj.ModifiedAt;
            CreatedAt = notebookBobj.CreatedAt;
            NoteCount = notebookBobj.NoteCount;
            NoteBookCover= notebookBobj.NoteBookCover;
        }
    }
}
