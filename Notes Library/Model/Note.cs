using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Models
{
    public class Note
    {
       
        [PrimaryKey,AutoIncrement]
        public  int NoteId { get; set; }
        
        public virtual string Title { get; set; }

        public int CreatedBy { get; set; }

        public  DateTime CreatedAt { get; set; }

        public virtual DateTime UpdatedAt { get; set; }

        
        public int? NotebookId { get; set; }

        public string NoteContent { get; set; }

        
        public virtual string NoteColor { get; set; }
        

    }
}
