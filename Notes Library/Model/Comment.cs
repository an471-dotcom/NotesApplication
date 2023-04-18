using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Models
{
    public class Comment
    {
        [PrimaryKey, AutoIncrement ]
        public int Id { get; set; }

       
        public string Content { get; set; }

        public int CommentedBy { get; set; }

        public DateTime CommentedAt { get; set; }

        public int CommentedTo { get; set; }

        public int? ParentId { get; set; }

        
    }

}
