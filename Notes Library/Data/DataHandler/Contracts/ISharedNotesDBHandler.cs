using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Data.Interface
{
    internal interface ISharedNotesDBHandler
    {
        Task<int> AddToSharedNotes(SharedNotes sharedNote);

        Task<SharedNotes> GetSharedNote(int userId, int noteId);

        Task<List<SharedNotes>> GetUserSharedNotes(int userId);

        Task<int> UnshareNote(int userId,int noteId);

        Task<int> DeleteSharedNote(int noteId);
    }
}
