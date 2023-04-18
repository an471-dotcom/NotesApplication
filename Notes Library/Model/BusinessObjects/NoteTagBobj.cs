using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Model.BussinessObjects
{
    public class NoteTagBobj
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public int NoteId { get; set; }

        public override string ToString()
        {
            return TagName;
        }
    }
}
