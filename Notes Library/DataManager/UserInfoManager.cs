using Notes_Library.Data.Interface;
using Notes_Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes_Library.Models;
using Notes_Library.Domain.UseCase;
using Windows.System;
using Notes_Library.Data.DataHandler;
using static SQLite.SQLite3;
using Notes_Library.Exceptions;
using Notes_Library.Domain;

namespace Notes_Library.DataManager
{
    public class UserInfoManager
    {
        private static UserInfoManager _instance = null;

        private UserInfoManager() { }
        public static UserInfoManager GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserInfoManager();
                }
                return _instance;
            }
        }
        private readonly IUserDBHandler _userDBHandler = UserDBHandler.GetInstance;
        public async Task<bool> VerifyUser(string email)
        {
            return await _userDBHandler.GetUserByEmail(email) != null;
        }

        public async void AddNewUser(string userName,string email,string password,string noteColor,IUsecaseCallback<AddUserResponse> callback)
        {
            try
            {
                var isDuplicate = await VerifyUser(email);
                if (isDuplicate)
                {
                    throw new DuplicateUserException("User Already Found");
                }
                else
                {
                    var user = await _userDBHandler.AddUser(new UserInfo { UserName = userName, Email = email, Password = password ,DefaultNoteColor = noteColor});
                    callback?.OnSuccess(new AddUserResponse { NewUser = user });
                }
                

                
            }
            catch(Exception e)
            {
                callback?.OnError(e);
            }
        }

        public async void GetUser(int userId,IUsecaseCallback<GetUserResponse> callback)
        {
            try
            {
                var user = await _userDBHandler.GetUserById(userId);
                callback.OnSuccess(new GetUserResponse { User= user });
            }
            catch(Exception e)
            {
                callback.OnError(e);
            }

           
        }

        public async Task<UserInfo> GetUserById(int userId)
        {
            return await _userDBHandler.GetUserById(userId);
        }


        public async void LoginUser(string email,string password,IUsecaseCallback<LoginUserResponse> callback)
        {
            try
            {
                
                var isFound = await VerifyUser(email);

                if (isFound)
                {
                    var user = await _userDBHandler.GetUserByEmail(email);

                    if (user.Password != password)
                    {
                       
                        throw new WrongPasswordException();
                    }
                    else
                    {
                        
                        callback.OnSuccess(new LoginUserResponse {User=user });

                    }
                }
                else
                {
                    throw new UserNotFoundException();
                }
      
            }
            catch(Exception ex)
            {
                callback.OnError(ex);
            }
        }
        public async void UpdateUser(int userId,string userName,string password,string noteColor,IUsecaseCallback<UpdateUserResponse> callback)
        {
            try
            {
                var user = await _userDBHandler.GetUserById(userId);
                user.DefaultNoteColor = noteColor;
                user.Password = password; 
                user.UserName= userName;
                await _userDBHandler.UpdateUser(user);
                callback.OnSuccess(new UpdateUserResponse { UpdatedUser=user });
            }
            catch(Exception ex)
            {
                callback.OnError(ex);
            }
        }

        
    }
}
