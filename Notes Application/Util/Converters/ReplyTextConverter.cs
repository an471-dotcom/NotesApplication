using Notes_Application.Utility_Classes;
using Notes_Library.Model.BussinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Notes_Application.Converters
{
    public class ReplyTextConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var reply = (ReplyBobj)value;

            if(reply!= null)
            { 
 
                if (reply.IsParentCommentReply)
                {
                    return reply.Content;
                }
                else
                {
                    return $"@{reply.CommentedToUserName} {reply.Content}";
                }
                
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
