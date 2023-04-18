using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace Notes_Library.Models
{
    public class UserInfo
    {
        [PrimaryKey,AutoIncrement]
        public int UserId { get; set; }

       
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get;  set; }

        public string DefaultNoteColor { get; set; }

    }
}
