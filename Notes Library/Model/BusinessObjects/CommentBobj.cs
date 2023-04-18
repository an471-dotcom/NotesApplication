using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Model.BussinessObjects
{
    public class CommentBobj:Comment
    {
        

        public string CommentedByUsername { get; set; }

        public List<ReplyBobj> Replies { get; set; }

        public CommentBobj(Comment comment)
        {
            Id = comment.Id;
            Content = comment.Content;
            CommentedBy = comment.CommentedBy;
            CommentedAt = comment.CommentedAt;
            CommentedTo = comment.CommentedTo;
            ParentId = comment.ParentId;

        }
    }
}
