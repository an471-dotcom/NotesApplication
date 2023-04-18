using Notes_Library.Data.Interface;
using Notes_Library.Data;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Notes_Library.Domain.UseCase;
using Windows.System;
using Notes_Library.Data.DataHandler;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Xaml.Controls;
using Notes_Library.Data.DataHandler.Contracts;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Domain;

namespace Notes_Library.DataManager
{
    public class SharedNoteManager
    {
        private static SharedNoteManager _instance = null;

        private SharedNoteManager() { }
        public static SharedNoteManager GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SharedNoteManager();
                }
                return _instance;
            }
        }

        private readonly ISharedNotesDBHandler _sharedDBHandler = SharedNoteDBHandler.GetInstance;
        private readonly IReactionDBHandler _reactionDBHandler = ReactionDBHandler.GetInstance;
        private readonly ICommentDBHandler _commentDBHandler = CommentDBHandler.GetInstance;
        private readonly IUserDBHandler _userDBHandler = UserDBHandler.GetInstance;
        private readonly CommentsManager _commentsManager = CommentsManager.GetInstance;
        public async Task<bool> IsShared(int userId, int noteId)
        {
            return await _sharedDBHandler.GetSharedNote(userId, noteId) != null;
        }

        public async void ShareNote(IEnumerable<string> emailList, int noteId,IUsecaseCallback<ShareNoteResponse> callback)
        {
            try
            {
                List<UserInfo> sharedList = new List<UserInfo>();
                foreach(var email in emailList)
                {
                    var user = (await _userDBHandler.GetUserByEmail(email));
                    var result = await _sharedDBHandler.AddToSharedNotes(new SharedNotes
                    {
                        NoteId = noteId,
                        UserId = user.UserId,
                    });

                    if(result==1)
                    {
                        sharedList.Add(user);
                    }
                }
                
                callback?.OnSuccess(new ShareNoteResponse { SharedList = sharedList }); 
            }
            catch(Exception ex)
            {
                callback.OnError(ex);
            }
        }

        public async void UnshareNote(int userId,int noteId,IUsecaseCallback<UnshareNoteResponse> callback)
        {
            try
            {
                await _sharedDBHandler.UnshareNote(userId, noteId);
                await _reactionDBHandler.DeleteUserReactionToNote(userId, noteId);
                await _commentDBHandler.DeleteUserCommentsToNote(userId, noteId);
                var comments = await _commentDBHandler.GetNoteComments(noteId);
                List<CommentBobj> noteComments = new List<CommentBobj>();
                foreach(var comment in comments)
                {
                    noteComments.Add(await _commentsManager.CommentToCommentBobj(comment));
                }
                callback?.OnSuccess(new UnshareNoteResponse { UserId = userId , NoteId =noteId, Comments = noteComments});
            }
            catch(Exception e)
            {
                callback?.OnError(e);
            }
        }

        
    }
}
