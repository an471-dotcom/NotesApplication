using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Data.Interface
{
    internal interface IFavoritesDBHandler
    {
        Task<Favorite> GetFavoriteNote(int userId, int noteId);

        Task<int> AddFavorite(Favorite favorite);

        Task<List<Favorite>> GetFavorites(int userId);

        Task<int> DeleteFavorite(int userId,int noteId);
    }
}
