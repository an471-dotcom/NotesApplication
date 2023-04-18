using Notes_Library.Data.DataAdapter.Contracts;
using Notes_Library.Data.DataAdapter;
using Notes_Library.Data.DataHandler.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes_Library.Domain.UseCase;
using Notes_Library.Domain.UseCase.CommentUseCases;
using Notes_Library.Models;
using System.Data;
using System.Runtime.InteropServices;

namespace Notes_Library.Data.DataHandler
{
    public class CommentDBHandler : ICommentDBHandler
    {
        private readonly IDatabaseAdapter _db;
        private CommentDBHandler()
        {
            _db = new DatabaseAdapter();
        }

      

        private static CommentDBHandler _instance = null;


        public static CommentDBHandler GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CommentDBHandler();
                }
                return _instance;
            }
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            await _db.InsertAsync(comment);
            return comment;
        }

        public async Task<Comment> GetCommentById(int commentId)
        {
            return (await _db.GetTableAsync<Comment>()).FirstOrDefault(c => c.Id.Equals(commentId));
        }

        public async Task<List<Comment>> GetNoteComments(int noteId)
        {
            return (await _db.GetTableAsync<Comment>()).Where(c => c.CommentedTo == noteId).ToList();
        }

        public async Task<List<Comment>> GetReplyComments(int parentId)
        {
            return (await _db.GetTableAsync<Comment>()).Where(c => c.ParentId == parentId).ToList();
        }

        public async Task<string> GetCommentedToUserName(int commentId)
        {
            var commenterId = (await _db.GetTableAsync<Comment>()).FirstOrDefault(c => c.Id == commentId).CommentedBy;
            return (await _db.GetTableAsync<UserInfo>()).FirstOrDefault(u => u.UserId == commenterId).UserName;

        }

        public async Task<int> DeleteComment(int commentId)
        {
            var replies = (await _db.GetTableAsync<Comment>()).Where(c => c.ParentId == commentId).ToList();
            foreach (var reply in replies)
            {
                await _db.DeleteAsync<Comment>(reply.Id);
            }
            return await _db.DeleteAsync<Comment>(commentId);
        }

        public async Task<int> DeleteNoteComments(int noteId)
        {
            var comments = (await _db.GetTableAsync<Comment>()).Where(c => c.CommentedTo == noteId).ToList();
            var totalRowDeleted = 0;
            foreach (var comment in comments)
            {
                var replies = (await _db.GetTableAsync<Comment>()).Where(c => c.ParentId == comment.Id).ToList();
                foreach (var reply in replies)
                {
                    await _db.DeleteAsync<Comment>(reply.Id);
                }

                totalRowDeleted += await _db.DeleteAsync<Comment>(comment.Id);
            }
            return totalRowDeleted == comments.Count ? 1 : 0;
        }

        public async Task<int> DeleteReply(int replyId)
        {
            var replies = (await _db.GetTableAsync<Comment>()).Where(c => c.CommentedTo == replyId).ToList();

            if(replies.Count() == 0)
            {
                return await _db.DeleteAsync<Comment>(replyId);
            }
            
            
            foreach (var reply in replies)
            {
                 
                await DeleteReply(reply.Id);
            }
            return await _db.DeleteAsync<Comment>(replyId);

        }
        public async Task<int> DeleteUserCommentsToNote(int userId, int noteId)
        {

            var userComments =(await _db.GetTableAsync<Comment>()).Where(c => c.CommentedTo == noteId && c.CommentedBy == userId).ToList();

            foreach (var comment in userComments)
            {
                await DeleteComment(comment.Id);
            }
            
            var comments = (await _db.GetTableAsync<Comment>()).Where(c => c.CommentedTo == noteId);

            foreach (var comment in comments)
            {
                var replies = (await _db.GetTableAsync<Comment>()).Where(c => c.ParentId == comment.Id).ToList();
                foreach (var reply in replies)
                {
                    if (reply.CommentedBy == userId)
                    {
                        await _db.DeleteAsync<Comment>(reply.Id);
                    }
                }
            }
            return 1;
        }
    }

}
