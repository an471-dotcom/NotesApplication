using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Data.DataHandler.Contracts
{
    public interface ICommentDBHandler
    {
        Task<Comment> AddComment(Comment comment);

        Task<List<Comment>> GetNoteComments(int noteId);

        Task<List<Comment>> GetReplyComments(int parentId);
        Task<string> GetCommentedToUserName(int commentId);

        Task<Comment> GetCommentById(int commentId);

        Task<int> DeleteComment(int commentId);

        Task<int> DeleteNoteComments(int noteId);

        Task<int> DeleteReply(int replyId);

        Task<int> DeleteUserCommentsToNote(int userId, int noteId);
    }
}
