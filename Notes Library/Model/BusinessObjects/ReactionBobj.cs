using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Model.BussinessObjects
{
    public class ReactionBobj:Reaction
    {
        public UserInfo ReactedUser { get; set; }

        public ReactionBobj(Reaction reaction)
        {
            this.Id= reaction.Id;
            this.ReactedBy = reaction.ReactedBy;
            this.ReactedTo = reaction.ReactedTo;
            this.ReactionType = reaction.ReactionType;
        }
    }
}
