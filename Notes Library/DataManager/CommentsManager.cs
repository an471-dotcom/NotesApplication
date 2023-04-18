using Notes_Library.Data.DataHandler;
using Notes_Library.Data.DataHandler.Contracts;
using Notes_Library.Data.Interface;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Domain.UseCase.CommentUseCase;
using Notes_Library.Domain.UseCase.CommentUseCases;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.DataManager
{
    public class CommentsManager
    {
        private static CommentsManager _instance = null;

        private CommentsManager() { }
        public static CommentsManager GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CommentsManager();
                }
                return _instance;
            }
        }
        private ICommentDBHandler _commentDBHandler = CommentDBHandler.GetInstance;
        private readonly IUserDBHandler _userDBHandler = UserDBHandler.GetInstance;

        public async void AddComment(int userId,int noteId,string commentText,int? parentId, IUsecaseCallback<AddCommentResponse> callback)
        {
            try
            {
                var comment = await _commentDBHandler.AddComment(new Comment
                {
                    Content = commentText,
                    CommentedAt = DateTime.Now,
                    CommentedBy = userId,
                    CommentedTo = noteId,
                    ParentId = parentId,
                });
                var commentBobj = await CommentToCommentBobj(comment);
                callback?.OnSuccess(new AddCommentResponse { Comment= commentBobj });

            }
            catch(Exception e) 
            {
                callback?.OnError(e);
            }
        }

        public async void AddReply(int userId,int repliedTo,string replytext,int parentId,IUsecaseCallback<AddReplyResponse> callback)
        {
            try
            {
                var reply = await _commentDBHandler.AddComment(new Comment
                {
                    Content = replytext,
                    CommentedAt = DateTime.Now,
                    CommentedBy = userId,
                    CommentedTo = repliedTo,
                    ParentId = parentId,
                });
                var replyBobj = await ReplyToReplyBobj(reply);

                callback?.OnSuccess(new AddReplyResponse { Reply = replyBobj });
            }
            catch(Exception e)
            {
                callback?.OnError(e);
            }
        }

        public async void DeleteComment(int commentId,IUsecaseCallback<DeleteCommentResponse> callback)
        {
            try
            {
                await _commentDBHandler.DeleteComment(commentId);
                callback?.OnSuccess(new DeleteCommentResponse { DeletedCommentId= commentId });
            }
            catch(Exception e)
            {
                callback?.OnError(e);
            }
        }

        public async void DeleteReply(int replyId,IUsecaseCallback<DeleteReplyResponse> callback)
        {
            try
            {
                var commentId = (await _commentDBHandler.GetCommentById(replyId)).ParentId;
                await _commentDBHandler.DeleteReply(replyId);
                callback?.OnSuccess(new DeleteReplyResponse { DeletedReplyParentId = (int)commentId, DeletedReplyId = replyId });
            }
            catch(Exception e)
            {
                callback?.OnError(e);
            }
        }

        public async Task<CommentBobj> CommentToCommentBobj(Comment comment)
        { 
            List<ReplyBobj> repliesBobj = new List<ReplyBobj>();
            var replies = (await _commentDBHandler.GetReplyComments(comment.Id));
            foreach(var reply in replies)
            {   
                repliesBobj.Add(await ReplyToReplyBobj(reply));
            }
            return new CommentBobj(comment)
            {
                CommentedByUsername = (await _userDBHandler.GetUserById(comment.CommentedBy)).UserName,
                Replies = repliesBobj
            };
            
        }

        public async Task<ReplyBobj> ReplyToReplyBobj(Comment comment)
        {
            

            return new ReplyBobj(comment)
            {
                CommenterUsername = (await _userDBHandler.GetUserById(comment.CommentedBy)).UserName,
                CommentedToUserName = (await _commentDBHandler.GetCommentedToUserName(comment.CommentedTo)),
                CommentedToUserId = (await _commentDBHandler.GetCommentById(comment.CommentedTo)).CommentedBy,
                IsParentCommentReply = comment.CommentedTo == comment.ParentId,
            };

            
        }
    }
}
