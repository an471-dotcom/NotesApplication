using Notes_Library.Data.DataAdapter.Contracts;
using Notes_Library.Data.DataAdapter;
using Notes_Library.Data.DataHandler.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes_Library.Models;

namespace Notes_Library.Data.DataHandler
{
    public class ReactionDBHandler:IReactionDBHandler
    {
        private readonly IDatabaseAdapter _db;
        private ReactionDBHandler()
        {
            _db = new DatabaseAdapter();
        }
        private static ReactionDBHandler _instance = null;

        public static ReactionDBHandler GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReactionDBHandler();
                }
                return _instance;
            }
        }

        public async Task<Reaction> AddReaction(Reaction reaction)
        {
           await _db.InsertAsync(reaction);
            return reaction;
        }

        public async Task<List<Reaction>> GetNoteReactions(int noteId)
        {
            return (await _db.GetTableAsync<Reaction>()).Where(r => r.ReactedTo == noteId).ToList();
        }

        public async Task<int> DeleteReaction(int reactionId)
        {
            
            return (await _db.DeleteAsync<Reaction>(reactionId));
        }

        public async Task<Reaction> GetReaction(int reactedBy,int reactedTo)
        {
            return (await _db.GetTableAsync<Reaction>()).FirstOrDefault(r => r.ReactedBy == reactedBy && r.ReactedTo == reactedTo);
        }

        public async Task<Reaction> UpdateUserReaction(Reaction reaction)
        {
            await _db.UpdateAsync(reaction);
            return reaction;    
        }

        public async Task<int> DeleteNoteReactions(int noteId)
        {
            var reactions = (await _db.GetTableAsync<Reaction>()).Where(r => r.ReactedTo == noteId).ToList();
            var totalRowsDeleted = 0;
            foreach (var reaction in reactions)
            {
                totalRowsDeleted += (await _db.DeleteAsync<Reaction>(reaction.Id));
            }
            return totalRowsDeleted == reactions.Count ?1 :0;
        }

        public async Task<int> DeleteUserReactionToNote(int userId,int noteId)
        {
            var reaction = (await _db.GetTableAsync<Reaction>()).FirstOrDefault(r => r.ReactedTo == noteId && r.ReactedBy == userId);
           if(reaction!=null)
            {
                return await _db.DeleteAsync<Reaction>(reaction.Id);
            }
           return 0;
           
        }

    }
}
