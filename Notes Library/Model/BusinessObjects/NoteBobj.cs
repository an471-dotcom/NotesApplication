using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Model.BussinessObjects
{
    public class NoteBobj : Note
    {
        
        public virtual bool IsFavorite { get; set; }

        public string NotebookName { get; set; }
        public bool IsSharedNote { get; set; }

        public List<ReactionBobj> NoteReactions { get; set; }

        public List<Tag> NoteTags { get; set; }

        public List<UserInfo> SharedUsers { get; set; }

        public List<CommentBobj> NoteComments { get; set; }
        

        public NoteBobj() { }
        public NoteBobj(NoteBobj noteBobj)
        {
            this.NoteId = noteBobj.NoteId;
            this.NoteContent = noteBobj.NoteContent;
            this.CreatedBy = noteBobj.CreatedBy;
            this.CreatedAt = noteBobj.CreatedAt;
            this.UpdatedAt = noteBobj.UpdatedAt;
            this.NotebookName = noteBobj.NotebookName;
            this.IsFavorite = noteBobj.IsFavorite;
            this.IsSharedNote = noteBobj.IsSharedNote;
            this.NotebookId = noteBobj.NotebookId;
            this.NoteColor = noteBobj.NoteColor;
            this.Title = noteBobj.Title;
            this.SharedUsers= noteBobj.SharedUsers;
            this.NoteComments= noteBobj.NoteComments;
            this.NoteReactions= noteBobj.NoteReactions;
        }
    }
}
