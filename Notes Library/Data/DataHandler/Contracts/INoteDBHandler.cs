using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Data.Interface
{
    internal interface INoteDBHandler
    {
        Task<Note> AddNote(Note note);

        Task<List<Note>> GetUserNotes(int userId);

        Task<int> DeleteNote(int noteId);

        Task<List<Note>> GetNotebookNotes(int notebookId);

        Task<Note> GetNoteById(int noteId);

        Task<Note> UpdateNote(Note note);

        Task<int> NoteCount(int notebookId);
        Task<List<UserInfo>> GetAllUnsharedUsers(int noteId, int userId);
        Task<List<UserInfo>> GetNotesharedUsers(int noteId);


    }
}
