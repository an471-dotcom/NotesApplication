using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Model.BussinessObjects
{
    public class ReplyBobj:Comment
    {
        

        public string CommenterUsername { get; set; }

        public int CommentedToUserId { get; set; }
        
        public string CommentedToUserName { get; set; }

        public bool IsParentCommentReply { get; set; }

        public ReplyBobj(Comment reply) 
        { 
            this.Id= reply.Id;
            this.Content= reply.Content;
            this.CommentedBy= reply.CommentedBy;
            this.ParentId= reply.ParentId;
            this.CommentedTo= reply.CommentedTo;
            this.CommentedAt= reply.CommentedAt;

        }

    }
}
