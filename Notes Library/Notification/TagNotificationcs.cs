using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Notification
{
    public static class TagNotificationcs
    {
        public static event Action<Tag> TagUpdated;

        public static void InvokeTagUpdated(Tag tag)
        {
            TagUpdated?.Invoke(tag);
        }
    }
}
