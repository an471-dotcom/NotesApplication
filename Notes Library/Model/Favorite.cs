using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Models
{
    internal class Favorite
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NoteId { get; set; }

       
    }
}
