using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Notes_Library.Models
{
    public enum Reactions
    {
        like,
        Celebrate,
        Sad,
        Heart,
        Happy
    }
    public class Reaction
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int ReactedTo { get; set; }

        [Column("Reactions")]
        public Reactions ReactionType { get; set; }

        public int ReactedBy { get; set; }


    }

    public class ReactionIcon
    {
        public Reactions Name;
        public string Glyph;
    }
}
