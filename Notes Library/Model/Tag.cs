using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Models
{
    public class Tag
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public virtual string Name { get; set; }
        public int UserId { get; set; }

        public virtual string TagColor { get; set; }

        public Tag(Tag tag)
        {
            this.Id= tag.Id;
            this.UserId= tag.UserId;
            this.Name= tag.Name;
            this.TagColor= tag.TagColor;
        }

        public Tag()
        {

        }
        public override string ToString()
        {
            return Name;
        }
    }
    
}
