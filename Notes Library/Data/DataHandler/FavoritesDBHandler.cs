using Notes_Library.Data.DataAdapter.Contracts;
using Notes_Library.Data.DataAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes_Library.Models;
using Notes_Library.Data.Interface;

namespace Notes_Library.Data.DataHandler
{
    internal class FavoritesDBHandler:IFavoritesDBHandler
    {
        private readonly IDatabaseAdapter _db;
        private FavoritesDBHandler()
        {
            _db = new DatabaseAdapter();
        }
        private static FavoritesDBHandler _instance = null;

        public static FavoritesDBHandler GetInstance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new FavoritesDBHandler();
                }
                return _instance;
            }
        }

        public async Task<Favorite> GetFavoriteNote(int userId, int noteId)
        {
           
            return (await _db.GetTableAsync<Favorite>()).FirstOrDefault(fn => fn.NoteId == noteId && fn.UserId == userId);
        }
        public async Task<int> AddFavorite(Favorite favorite)
        {
 
            return await _db.InsertAsync(favorite);
        }

        public async Task<List<Favorite>> GetFavorites(int userId)
        {
           
            return (await _db.GetTableAsync<Favorite>()).Where(f => f.UserId == userId).ToList();
        }

        public async Task<int> DeleteFavorite(int userId, int noteId)
        {
            var userFavorite = (await _db.GetTableAsync<Favorite>()).FirstOrDefault(fn => fn.UserId == userId && fn.NoteId == noteId);
            if(userFavorite != null)
            {
                return await _db.DeleteAsync<Favorite>(userFavorite.Id);
            }
            else
            {
                return 0;
            }
            
        }
    }
}
