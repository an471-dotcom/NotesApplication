using Notes_Library.Data;
using Notes_Library.Data.DataHandler;
using Notes_Library.Data.DataHandler.Contracts;
using Notes_Library.Data.Interface;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Domain.UseCase.Notebook;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.System;


namespace Notes_Library.DataManager
{
    public class NotebookManager
    {
        private static NotebookManager _instance = null;

        private NotebookManager() { }
        public static NotebookManager GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NotebookManager();
                }
                return _instance;
            }
        }
        private readonly INotebookDBHandler _notebookDBHandler = NotebookDBHandler.GetInstance;
        private readonly INoteDBHandler _noteDBHandler = NotesDBHandler.GetInstance;
        private readonly IFavoritesDBHandler _favoritesDBHandler = FavoritesDBHandler.GetInstance;
        private readonly ISharedNotesDBHandler _sharedNotesDBHandler = SharedNoteDBHandler.GetInstance;
        private readonly ITagDBHandler _tagDBHandler= TagDBHandler.GetInstance;

        public async void CreateNotebook(string notebookName, int createdBy, string notebookCover,IUsecaseCallback<CreateNotebookResponse> callback)
        {
            try
            {
                var notebook =await _notebookDBHandler.AddNotebook(new NoteBook
                {
                    Name = notebookName,
                    CreatedBy = createdBy,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    NoteBookCover = notebookCover
                    
                });
                NoteBookBobj notebookBobj = await NotebookToNotebookBobj(notebook);
                callback.OnSuccess(new CreateNotebookResponse { Notebook = notebookBobj });
            }
            catch(Exception ex)
            {
                callback.OnError(ex);
            }

        }

        

        public async void RenameNotebook(string name,int notebookId,IUsecaseCallback<RenameNoteBookResponse> callback)
        {
            try
            {
                var notebook = await _notebookDBHandler.GetNotebookById(notebookId);
                notebook.Name = name;
                notebook.ModifiedAt = DateTime.Now;
                var UpdatedNotebookBobj = await NotebookToNotebookBobj(await _notebookDBHandler.UpdateNotebook(notebook));
                callback.OnSuccess(new RenameNoteBookResponse { UpdatedNotebook = UpdatedNotebookBobj});
            }
            catch(Exception e)
            {
                callback.OnError(e);
            }
            

        }

        public async void ChangeNoteBookCover(int notebookId,string notebookCover,IUsecaseCallback<ChangeNoteBookCoverResponse> callback)
        {
            try
            {
                var notebook = await _notebookDBHandler.GetNotebookById(notebookId);
                notebook.NoteBookCover = notebookCover;
                notebook.ModifiedAt = DateTime.Now;
                await _notebookDBHandler.UpdateNotebook(notebook);
                callback.OnSuccess(new ChangeNoteBookCoverResponse { NoteBookCover = notebookCover,NoteBookId = notebookId});
            }
            catch(Exception e)
            {
                callback.OnError(e);
            }
        }
        public async void DeleteNotebook(int notebookId,IUsecaseCallback<DeleteNoteBookResponse> callback)
        {
           try
           {
                
                var notes = await _noteDBHandler.GetNotebookNotes(notebookId);
                foreach(var note in notes)
                {
                    await DeleteNote(note.CreatedBy,note.NoteId);
                }
                await _notebookDBHandler.DeleteNotebook(notebookId);
                callback.OnSuccess(new DeleteNoteBookResponse { NotebookId = notebookId });
            }
            catch(Exception ex)
            {
                callback.OnError(ex);
            }

        }

        public async void GetNotebooks(int userId,IUsecaseCallback<GetNoteBooksResponse> callback)
        {
            try
            {
                var notebooksBobj = new List<NoteBookBobj>();
                var notebooks = await _notebookDBHandler.GetUserNotebooks(userId);
                foreach(var notebook in notebooks)
                {
                    notebooksBobj.Add(await NotebookToNotebookBobj(notebook));
                }
                callback.OnSuccess(new GetNoteBooksResponse { Notebooks = notebooksBobj });
            }
            catch(Exception ex)
            {
                callback?.OnError(ex);
            }
            
        }


        public async Task<string> GetNotebookTitle(int notbookId)
        {
            var notebook = await _notebookDBHandler.GetNotebookById(notbookId);
            return notebook.Name;
        }

        

        public async Task<int> NotebookNotesCount(int notebookId)
        {
            return await _noteDBHandler.NoteCount(notebookId);
        }

        

        private async Task DeleteNote(int userId,int noteId)
        {
            await _noteDBHandler.DeleteNote(noteId);
            await _favoritesDBHandler.DeleteFavorite(userId, noteId);
            await _sharedNotesDBHandler.UnshareNote(userId,noteId);
            await _tagDBHandler.DeleteAllTagsFromNote(noteId);
        }

        public async Task UpdateNotebook(DateTime modifiedAt,int notebookId)
        {
            var notebook = await _notebookDBHandler.GetNotebookById(notebookId);
            notebook.ModifiedAt = modifiedAt;
            await _notebookDBHandler.UpdateNotebook(notebook);
        }

        public async Task<NoteBookBobj> NotebookToNotebookBobj(NoteBook noteBook)
        {
            var noteCount = await _noteDBHandler.NoteCount(noteBook.NotebookId);
            return new NoteBookBobj
            {
                Name = noteBook.Name,
                CreatedAt = noteBook.CreatedAt,
                CreatedBy = noteBook.CreatedBy,
                NoteCount = noteCount,
                NotebookId = noteBook.NotebookId,
                ModifiedAt = noteBook.ModifiedAt,
                NoteBookCover = noteBook.NoteBookCover
            };
        }
        
       
    }
}
