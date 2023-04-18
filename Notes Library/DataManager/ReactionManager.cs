using Notes_Library.Data.DataHandler;
using Notes_Library.Data.DataHandler.Contracts;
using Notes_Library.Data.Interface;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Domain.UseCase.Reaction;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.DataManager
{
    public class ReactionManager
    {
        private static ReactionManager _instance = null;

        private ReactionManager() { }   
        public static ReactionManager GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReactionManager();
                }
                return _instance;
            }
        }
        private IReactionDBHandler _reactionDBHandler = ReactionDBHandler.GetInstance;
        private readonly IUserDBHandler _userDBHandler = UserDBHandler.GetInstance;
        public async void AddReaction(int reactedBy,int reactedTo,Reactions reactionType,IUsecaseCallback<AddReactionResponse> callback)
        {
            try
            {
                var reaction = await _reactionDBHandler.GetReaction(reactedBy, reactedTo);
                if (reaction == null)
                {
                    reaction = await _reactionDBHandler.AddReaction(new Reaction
                    {
                        ReactedBy = reactedBy,
                        ReactedTo = reactedTo,
                        ReactionType = reactionType,
                    });
                }
                else
                {
                    reaction.ReactionType = reactionType;
                    reaction = await _reactionDBHandler.UpdateUserReaction(reaction);
                }
                var reactionBobj = await ConvertToReactionBobj(reaction);
                
                callback?.OnSuccess(new AddReactionResponse { Reaction= reactionBobj });
                
            }
            catch(Exception e)
            {
                callback?.OnError(e);
            }
        }

        public async void RemoveReaction(int reactionId,IUsecaseCallback<RemoveReactionResponse> callback)
        {
            try
            {
                await _reactionDBHandler.DeleteReaction(reactionId);
                callback?.OnSuccess(new RemoveReactionResponse { RemovedReactionId = reactionId });
            }
            catch(Exception e)
            {
                callback?.OnError(e);
            }
        }

        public async Task<ReactionBobj> ConvertToReactionBobj(Reaction reaction)
        {
            return new ReactionBobj(reaction)
            {
                ReactedUser = await _userDBHandler.GetUserById(reaction.ReactedBy)
            };
        }

    }
}
