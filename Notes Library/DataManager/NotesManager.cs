using Notes_Library.Data;
using Notes_Library.Data.DataHandler;
using Notes_Library.Data.DataHandler.Contracts;
using Notes_Library.Data.Interface;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace Notes_Library.DataManager
{
    public interface INotesManager
    {

    }
    public class NotesManager
    {
        private static NotesManager _instance = null;

        private NotesManager() { }
        public static NotesManager GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NotesManager();
                }
                return _instance;
            }
        }

        private readonly INoteDBHandler _noteDBHandler = NotesDBHandler.GetInstance;
        private readonly INotebookDBHandler _notebookDBHandler = NotebookDBHandler.GetInstance;
        private readonly IFavoritesDBHandler _favoritesDBHandler = FavoritesDBHandler.GetInstance;
        private readonly ISharedNotesDBHandler _sharedDBHandler = SharedNoteDBHandler.GetInstance;
        private readonly ITagDBHandler _tagDBHandler = TagDBHandler.GetInstance;
        private readonly ICommentDBHandler _commentDBHandler = CommentDBHandler.GetInstance;
        private readonly IReactionDBHandler _reactionDBHandler = ReactionDBHandler.GetInstance;
        private readonly CommentsManager _commentsManager = CommentsManager.GetInstance;
        private readonly ReactionManager _reactionsManager = ReactionManager.GetInstance;
        public async void CreateNewNote(string title, string content, int userId, string bgColor, int? notebookId, IUsecaseCallback<CreateNoteResponse> callback)
        {
            try
            {
                if (notebookId == 0)
                {
                    notebookId = null;
                }

                var newNote = await _noteDBHandler.AddNote(new Note
                {
                    Title = title,
                    NoteContent = content,
                    CreatedBy = userId,
                    NotebookId = notebookId,
                    NoteColor = bgColor,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                });

                if (notebookId != null)
                {
                    await UpdateNotebook(newNote.CreatedAt, (int)notebookId);
                }

                var bobjNote = await NoteToNoteBobj(newNote.CreatedBy, newNote);
                callback?.OnSuccess(new CreateNoteResponse { Note = bobjNote });

            }
            catch (Exception ex)
            {
                callback?.OnError(ex);
            }


        }

        public async void DeleteNote(int userId, int noteId, IUsecaseCallback<DeleteNoteResponse> Callback)
        {
            try
            {
                
                await _favoritesDBHandler.DeleteFavorite(userId, noteId);
                await _sharedDBHandler.DeleteSharedNote(noteId);
                await _tagDBHandler.DeleteAllTagsFromNote(noteId);
                await _reactionDBHandler.DeleteNoteReactions(noteId);
                await _commentDBHandler.DeleteNoteComments(noteId);
                await _noteDBHandler.DeleteNote(noteId);

                Callback.OnSuccess(new DeleteNoteResponse { NoteId = noteId });
            }
            catch (Exception e)
            {
                Callback.OnError(e);
            }
        }

        public async void GetNotes(int userId,  IUsecaseCallback<GetAllNotesResponse> callback)
        {
            try
            {
               var notes = await _noteDBHandler.GetUserNotes(userId);
                         
                List<NoteBobj> notesBobj = new List<NoteBobj>();
                foreach (var note in notes)
                {
                    notesBobj.Add(await NoteToNoteBobj(userId, note));
                }
                callback.OnSuccess(new GetAllNotesResponse { Notes = notesBobj });
            }
            catch (Exception e)
            {
                callback.OnError(e);
            }

        }

      
        public async void GetNotesWithTag(int tagId, IUsecaseCallback<GetNotesByTagResponse> callback)
        {
            try
            {
                var tag = await _tagDBHandler.GetTag(tagId);
                var notes = await _tagDBHandler.GetNotesWithTag(tag.Id);
                List<NoteBobj> notesBobj = new List<NoteBobj>();
                foreach (var note in notes)
                {
                    notesBobj.Add(await NoteToNoteBobj(tag.UserId, note));
                }
                callback.OnSuccess(new GetNotesByTagResponse { Notes = notesBobj, TagName = tag.Name });

            }
            catch (Exception e)
            {
                callback.OnError(e);
            }
        }
        public async void UpdateNote(int noteId ,string title, string content, string noteBg, IUsecaseCallback<UpdateNoteResponse> callback)
        {
            try
            {
                var note = await _noteDBHandler.GetNoteById(noteId);
                note.Title = title;
                note.NoteContent = content;
                note.NoteColor = noteBg;
                note.UpdatedAt = DateTime.Now;
                var updatedNote = await _noteDBHandler.UpdateNote(note);
                var updatedNotebobj = await NoteToNoteBobj(updatedNote.CreatedBy, updatedNote);
               
                callback.OnSuccess(new UpdateNoteResponse { Note = updatedNotebobj });

                
            }
            catch (Exception e)
            {
                callback.OnError(e);
            }


        }

        public async void GetNotebookNotes(int notebookId, IUsecaseCallback<GetNotebookNotesResponse> callback)
        {
            try
            {
                List<NoteBobj> bobjNotes = new List<NoteBobj>();
                var notebookTitle = await _notebookDBHandler.GetNotebookName(notebookId);
                var notes = await _noteDBHandler.GetNotebookNotes(notebookId);
                foreach (var note in notes)
                {
                    bobjNotes.Add(await NoteToNoteBobj(note.CreatedBy, note));
                }
                callback.OnSuccess(new GetNotebookNotesResponse { NotebookTitle = notebookTitle, Notes = bobjNotes });
            }
            catch (Exception e)
            {
                callback.OnError(e);
            }

        }


        public async Task<bool> IsFavorite(int userId, int noteId)
        {
            return await _favoritesDBHandler.GetFavoriteNote(userId, noteId) != null;
        }

        public async void MakeFavorite(int userId, int noteId, IUsecaseCallback<FavoriteNoteResponse> callback)
        {
            try
            {
                var result = await _favoritesDBHandler.AddFavorite(new Favorite { NoteId = noteId, UserId = userId });
                callback.OnSuccess(new FavoriteNoteResponse { NoteId = noteId });
            }
            catch (Exception e)
            {
                callback.OnError(e);
            }
        }

        public async void RemoveFavorite(int userId, int noteId, IUsecaseCallback<UnFavoriteNoteResponse> callback)
        {
            try
            {
                int result = await _favoritesDBHandler.DeleteFavorite(userId, noteId).ConfigureAwait(false);
                callback.OnSuccess(new UnFavoriteNoteResponse {  NoteId = noteId });
            }
            catch (Exception e)
            {
                callback.OnError(e);
            }


        }
        public async Task UpdateNotebook(DateTime modifiedAt, int notebookId)
        {
            var notebook = await _notebookDBHandler.GetNotebookById(notebookId);
            notebook.ModifiedAt = modifiedAt;
            await _notebookDBHandler.UpdateNotebook(notebook);
        }

        public async void GetUserSuggestion(int userId, int noteId, IUsecaseCallback<GetUserSuggestionResponse> callback)
        {
            try
            {
                var unSharedUsers = await _noteDBHandler.GetAllUnsharedUsers(noteId,userId);
                callback?.OnSuccess(new GetUserSuggestionResponse { UserEmails = unSharedUsers });
            }
            catch (Exception e)
            {
                callback?.OnError(e);
            }
        }

        public async Task<List<UserInfo>> GetNoteSharedUsers(int noteId)
        {
            return await _noteDBHandler.GetNotesharedUsers(noteId);
            
        }

        public async Task<NoteBobj> NoteToNoteBobj(int userId, Note note)
        {
         
            var isFavorite = await IsFavorite(userId, note.NoteId);
            var isShared = userId != note.CreatedBy;
            var notebookName = note.NotebookId != null ? await _notebookDBHandler.GetNotebookName((int)note.NotebookId) : "-";
            var tags = await _tagDBHandler.GetNoteTags(note.NoteId);
            var sharedUsers = await GetNoteSharedUsers(note.NoteId);
            var reactions = await _reactionDBHandler.GetNoteReactions(note.NoteId);
            var comments = await _commentDBHandler.GetNoteComments(note.NoteId);
            List<CommentBobj> commentsBobj= new List<CommentBobj>();
            List<ReactionBobj> rectionsBobj = new List<ReactionBobj>();
            
            foreach(var comment in comments)
            {
                commentsBobj.Add(await _commentsManager.CommentToCommentBobj(comment));
            }
           commentsBobj = commentsBobj.OrderByDescending(c => c.CommentedAt).ToList();
            foreach(var reaction in reactions)
            {
                rectionsBobj.Add(await _reactionsManager.ConvertToReactionBobj(reaction));
            }
          
            return new NoteBobj
            {
                CreatedAt = note.CreatedAt,
                NoteColor = note.NoteColor,
                Title = note.Title,
                NoteId = note.NoteId,
                NotebookId = note.NotebookId,
                UpdatedAt = note.UpdatedAt,
                IsFavorite = isFavorite,
                IsSharedNote = isShared,
                NoteContent = note.NoteContent,
                CreatedBy = note.CreatedBy,
                NotebookName = notebookName,
                NoteTags = tags,
                SharedUsers = sharedUsers,
                NoteComments = commentsBobj,
                NoteReactions = rectionsBobj

            };

        }

        public Note NoteBobjToNote(NoteBobj noteBobj)
        {
            return new Note
            {
                NoteId = noteBobj.NoteId,
                CreatedBy = noteBobj.CreatedBy,
                NoteContent = noteBobj.NoteContent,
                UpdatedAt = noteBobj.UpdatedAt,
                Title = noteBobj.Title,
                CreatedAt = noteBobj.CreatedAt,
                NoteColor = noteBobj.NoteColor,
                NotebookId = noteBobj.NotebookId,
            };
        }

        


    }
}
