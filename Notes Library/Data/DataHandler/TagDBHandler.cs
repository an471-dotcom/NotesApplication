using Notes_Library.Data.DataAdapter.Contracts;
using Notes_Library.Data.DataAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes_Library.Models;
using Notes_Library.Data.DataHandler.Contracts;
using Windows.Services.Maps;
using Notes_Library.DataManager;

namespace Notes_Library.Data.DataHandler
{
    public class TagDBHandler : ITagDBHandler
    {
        private static TagDBHandler _instance = null;
        public static TagDBHandler GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TagDBHandler();
                }
                return _instance;
            }
        }
        private readonly IDatabaseAdapter _db;
        private TagDBHandler()
        {
            _db = new DatabaseAdapter();
        }

        public async Task<Tag> AddTag(Tag tag)
        {
            await _db.InsertAsync(tag);
            return tag;
        }

        public async Task<Tag> GetTag(int id)
        {
            return (await _db.GetTableAsync<Tag>()).FirstOrDefault(tag => tag.Id == id);
        }

        public async Task<List<Tag>> GetNoteTags(int noteId)
        {
            List<Tag> tags = new List<Tag>();
            var taggedNotes = (await _db.GetTableAsync<TaggedNotes>()).Where(tag => tag.NoteId== noteId).ToList();
            foreach(var taggednote in taggedNotes)
            {
                tags.Add(await GetTag(taggednote.TagId));
            }
            return tags;
        }

        public async Task<int> AddTagToNote(TaggedNotes taggedNotes)
        {
            return await _db.InsertAsync(taggedNotes);
        }

        public async Task<int> DeleteTag(int tagId)
        {
            var taggedNotes = (await _db.GetTableAsync<TaggedNotes>()).Where(tag => tag.TagId == tagId).ToList();
            foreach(var taggedNote in taggedNotes)
            {
                await _db.DeleteAsync<TaggedNotes>(taggedNote.Id);
            }
            return await _db.DeleteAsync<Tag>(tagId);
        }

        public async Task<int> DeleteNoteTag(int noteId,int tagId)
        {
            var taggedNote = (await _db.GetTableAsync<TaggedNotes>()).FirstOrDefault(t => t.TagId == tagId && t.NoteId==noteId);
            return await _db.DeleteAsync<TaggedNotes>(taggedNote.Id);
        }

        public async Task<List<Tag>> GetNoteTagSuggestion(string input,int userId)
        {
            return (await _db.GetTableAsync<Tag>())
                .Where(tag => tag.UserId == userId && tag.Name.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<Tag> GetTagByName(string tagName,int userId)
        {
            return (await _db.GetTableAsync<Tag>()).Where(t => t.UserId == userId && string.Equals(t.Name, tagName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public async Task<List<Tag>> GetUserTags(int userId)
        {
            return (await _db.GetTableAsync<Tag>()).Where(t => t.UserId == userId).ToList();
        }

        public async Task<bool> IsTagAlreadyAdded(int tagId,int noteId)
        {
            return (await _db.GetTableAsync<TaggedNotes>()).FirstOrDefault(t => t.TagId == tagId && t.NoteId == noteId) != null;
        }

        public async Task<List<Note>> GetNotesWithTag(int tagId)
        {
            List<Note> notes = new List<Note>();
            var taggedNotes = (await _db.GetTableAsync<TaggedNotes>()).Where(t => t.TagId == tagId).ToList();
            foreach(var taggedNote in taggedNotes)
            {
                var note = (await _db.GetTableAsync<Note>()).FirstOrDefault(n => n.NoteId == taggedNote.NoteId);
                notes.Add(note);
            }
            
            return notes;
        }

        public async Task DeleteAllTagsFromNote(int noteId)
        {
            var taggedNotes = (await _db.GetTableAsync<TaggedNotes>()).Where(t => t.NoteId == noteId).ToList();
            foreach(var taggedNote in taggedNotes)
            {
                await _db.DeleteAsync<TaggedNotes>(taggedNote.Id);
            }
        }

        public async Task<Tag> UpdateTag(Tag tag)
        {
            await _db.UpdateAsync(tag);
            return tag;
        }
    }
}
