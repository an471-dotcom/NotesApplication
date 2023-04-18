using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Models
{
    public class NoteBook
    {

        [PrimaryKey, AutoIncrement]
        public int NotebookId { get; set; }

        public virtual string Name { get; set; }

        public int CreatedBy { get; set; }
        public  DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public virtual string NoteBookCover { get; set; }
      

    }
}
