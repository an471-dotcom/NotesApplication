using Notes_Library.Data.DataAdapter.Contracts;
using Notes_Library.Data.DataAdapter;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes_Library.Data.Interface;
using System.ComponentModel.DataAnnotations;

namespace Notes_Library.Data.DataHandler
{
    internal class NotesDBHandler:INoteDBHandler
    {
        private static NotesDBHandler _instance = null;
        public static NotesDBHandler GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NotesDBHandler();
                }
                return _instance;
            }
        }

        private readonly IDatabaseAdapter _db;
        private NotesDBHandler()
        {
            _db = new DatabaseAdapter();
        }
       

        public async Task<Note> AddNote(Note note)
        {
            await _db.InsertAsync(note);
            return note;
        }
        public async Task<List<Note>> GetUserNotes(int userId)
        {
            var userNotes = (await _db.GetTableAsync<Note>()).Where(note => note.CreatedBy == userId).ToList();
            var sharedNotes = (await _db.GetTableAsync<SharedNotes>()).Where( note => note.UserId== userId).ToList();
            foreach(var sharedNote in sharedNotes)
            {
                userNotes.Add(await GetNoteById(sharedNote.NoteId));
            }
            return userNotes;
           
        }
        public async Task<int> DeleteNote(int noteId)
        {
            var note = (await _db.GetTableAsync<Note>()).FirstOrDefault(n => n.NoteId == noteId);
            return await _db.DeleteAsync<Note>(note.NoteId);
        }
        public async Task<List<Note>> GetNotebookNotes(int notebookId)
        {
            return (await _db.GetTableAsync<Note>()).Where(note => note.NotebookId == notebookId).ToList();
        }
        public async Task<Note> GetNoteById(int noteId)
        {
            return (await _db.GetTableAsync<Note>()).FirstOrDefault(n => n.NoteId == noteId);
        }
        public async Task<Note> UpdateNote(Note note)
        {
            await _db.UpdateAsync(note);
            return note;
        }

        public async Task<int> NoteCount(int notebookId)
        {
           
            return (await _db.GetTableAsync<Note>()).Where(n => n.NotebookId == notebookId).Count();
        }

        public async Task<List<UserInfo>> GetAllUnsharedUsers(int noteId,int userId)
        {
            var sharedUserIds = (await _db.GetTableAsync<SharedNotes>()).Where(sharedNote => sharedNote.NoteId == noteId).Select(sharedNote => sharedNote.UserId);
            var allusers = (await _db.GetTableAsync<UserInfo>()).Where(user => user.UserId !=userId).Select(user => user.UserId).ToList();
            var unSharedUserIds = allusers.Except(sharedUserIds);
            var unSharedUsers = new List<UserInfo>();
            foreach(var id in unSharedUserIds)
            {
                unSharedUsers.Add((await _db.GetTableAsync<UserInfo>()).FirstOrDefault(user => user.UserId == id));
            }
            return unSharedUsers;
            
        }

        public async Task<List<UserInfo>> GetNotesharedUsers(int noteId)
        {
            List<UserInfo> users = new List<UserInfo>();
            var sharedUserIds = (await _db.GetTableAsync<SharedNotes>()).Where(sharedNote => sharedNote.NoteId == noteId).Select(sharedNote => sharedNote.UserId).ToList();
            foreach(var userId in sharedUserIds)
            {
                users.Add((await _db.GetTableAsync<UserInfo>()).FirstOrDefault(user => user.UserId == userId));
            }
            return users;
        }


    }
}
