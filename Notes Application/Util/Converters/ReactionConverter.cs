using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Notes_Application.Converters
{
    public class ReactionConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var reaction = (Reactions)value;
            string reactionIcon = string.Empty;
            switch (reaction)
            {
                case Reactions.like:
                    reactionIcon = "\U0001F44D";
                    break;
                case Reactions.Happy:
                    reactionIcon = "\U0001F60A";
                    break;
                case Reactions.Celebrate:
                    reactionIcon = "\U0001F44F";
                    break;
                case Reactions.Sad:
                    reactionIcon = "\U0001F625";
                    break;
                case Reactions.Heart:
                    reactionIcon = "\u2665";
                    break;
            }
            return reactionIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
