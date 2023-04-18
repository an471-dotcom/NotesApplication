using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Data.DataHandler.Contracts
{
    internal interface IReactionDBHandler
    {
        Task<Reaction> AddReaction(Reaction reaction);

        Task<List<Reaction>> GetNoteReactions(int noteId);

        Task<Reaction> GetReaction(int reactedBy, int reactedTo);

        Task<Reaction> UpdateUserReaction(Reaction reaction);

        Task<int> DeleteReaction(int reactionId);

        Task<int> DeleteNoteReactions(int noteId);

        Task<int> DeleteUserReactionToNote(int userId, int noteId);

    }
}
