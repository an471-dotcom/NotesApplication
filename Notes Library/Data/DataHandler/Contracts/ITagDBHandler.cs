using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Data.DataHandler.Contracts
{
    public interface ITagDBHandler
    {
        Task<Tag> AddTag(Tag tag);
        Task<Tag> GetTag(int id);
        Task<List<Tag>> GetNoteTags(int noteId);
        Task<int> AddTagToNote(TaggedNotes taggedNotes);
        Task<List<Tag>> GetNoteTagSuggestion(string input, int userId);
        Task<int> DeleteNoteTag(int noteId, int tagId);
        Task<Tag> GetTagByName(string tagName, int userId);
        Task<bool> IsTagAlreadyAdded(int tagId, int noteId);
        Task<List<Tag>> GetUserTags(int userId);
        Task DeleteAllTagsFromNote(int noteId);
        Task<List<Note>> GetNotesWithTag(int tagId);
        Task<int> DeleteTag(int tagId);

        Task<Tag> UpdateTag(Tag tag);
    }
}
