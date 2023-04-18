using Notes_Library.Data.DataAdapter;
using Notes_Library.Data.DataAdapter.Contracts;
using Notes_Library.Data.Interface;
using Notes_Library.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Data.DataHandler
{
    public class UserDBHandler:IUserDBHandler
    {
        private static UserDBHandler _instance = null;
        public static UserDBHandler GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserDBHandler();
                }
                return _instance;
            }
        }
        private readonly IDatabaseAdapter _db;
        private UserDBHandler() 
        {
            _db = new DatabaseAdapter();
        }

        public async Task<UserInfo> GetUserByEmail(string email)
        {
            return (await _db.GetTableAsync<UserInfo>()).FirstOrDefault(t => t.Email.Equals(email));
        }
        public async Task<UserInfo> AddUser(UserInfo userInfo)
        {
            await _db.InsertAsync(userInfo);

            return userInfo;
        }
        public async Task<List<string>> GetEmailSuggestion(string input,int userId)
        {
            return (await _db.GetTableAsync<UserInfo>()).Where(user => user.Email.StartsWith(input,StringComparison.OrdinalIgnoreCase) && user.UserId != userId).Select(user => user.Email).ToList();
        }

        public async Task<UserInfo> GetUserById(int userId)
        {
         
            return (await _db.GetTableAsync<UserInfo>()).FirstOrDefault(user => user.UserId == userId);
        }

        public async Task<Note> UpdateNote(Note note)
        {
           
            await _db.UpdateAsync(note);
            return note;

        }

        public async Task<int> NoteCount(int notebookId)
        {
        
            return (await _db.GetTableAsync<Note>()).Where(n => n.NotebookId == notebookId).Count();
        }
        
        public async Task<int> UpdateUser(UserInfo userInfo)
        {
            return (await _db.UpdateAsync<UserInfo>(userInfo));
        }

        
    }
}
