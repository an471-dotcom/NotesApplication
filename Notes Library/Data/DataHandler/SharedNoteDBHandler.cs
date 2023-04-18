using Notes_Library.Data.DataAdapter.Contracts;
using Notes_Library.Data.DataAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes_Library.Models;
using Windows.ApplicationModel.DataTransfer;
using Notes_Library.Data.Interface;

namespace Notes_Library.Data.DataHandler
{
    public class SharedNoteDBHandler:ISharedNotesDBHandler
    {
        private static SharedNoteDBHandler _instance = null;
        public static SharedNoteDBHandler GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SharedNoteDBHandler();
                }
                return _instance;
            }
        }
        private readonly IDatabaseAdapter _db;
        private SharedNoteDBHandler()
        {
            _db = new DatabaseAdapter();
        }

        public async Task<int> AddToSharedNotes(SharedNotes sharedNote)
        {
            
           return await _db.InsertAsync(sharedNote);
        }

        public async Task<SharedNotes> GetSharedNote(int userId, int noteId)
        {
            return (await _db.GetTableAsync<SharedNotes>()).FirstOrDefault(s => s.UserId == userId && s.NoteId == noteId);
        }
        public async Task<List<SharedNotes>> GetUserSharedNotes(int userId)
        {
           
            return (await _db.GetTableAsync<SharedNotes>()).Where(s => s.UserId == userId).ToList();
        }




        public async Task<int> UnshareNote(int userId,int noteId)
        {
            var sharedNote = (await _db.GetTableAsync<SharedNotes>()).FirstOrDefault(s => s.UserId == userId && s.NoteId == noteId);
            if (sharedNote != null)
            {
                return await _db.DeleteAsync<SharedNotes>(sharedNote.Id);
            }
            return 0;
            
        }

        public async Task<int> DeleteSharedNote(int noteId)
        {
           
            var sharedNotes = (await _db.GetTableAsync<SharedNotes>()).Where(s => s.NoteId == noteId).ToList();
            var totalRowsDeleted = 0;
            foreach (var sharedNote in sharedNotes)
            {
                totalRowsDeleted += await _db.DeleteAsync<SharedNotes>(sharedNote.Id);
            }
            return totalRowsDeleted == sharedNotes.Count ? 1 : 0;

        }
    }
}
