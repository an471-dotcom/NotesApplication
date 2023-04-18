using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace Notes_Library.Data.Interface
{
    internal interface IUserDBHandler
    {
        Task<UserInfo> GetUserByEmail(string email);

        Task<UserInfo> AddUser(UserInfo userInfo);

        Task<List<string>> GetEmailSuggestion(string input,int userId);

        Task<UserInfo> GetUserById(int userId);

        Task<int> UpdateUser(UserInfo userInfo);

        //Task<List<UserInfo>> GetAllOtherUsers(int userId);


    }
}
