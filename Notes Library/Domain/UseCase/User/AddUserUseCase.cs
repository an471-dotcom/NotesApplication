using Notes_Library.DataManager;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase
{
    public class AddUserResponse
    {
        public UserInfo NewUser { get; set; }

    }

    public class AddUserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string DefaultNoteColor { get; set; }

    }

    public class AddUserUseCase : UseCaseBase<AddUserResponse>
    {
        private AddUserRequest _request;

        private UserInfoManager _userInfoManager;
        public AddUserUseCase(AddUserRequest request, IPresenterCallback<AddUserResponse> callback) : base(callback)
        {
            _request = request;

            _userInfoManager = UserInfoManager.GetInstance;
        }
        public override void Action()
        {
            _userInfoManager.AddNewUser(_request.UserName, _request.Email, _request.Password, _request.DefaultNoteColor, new AddNewUserUsecaseCallback(this));
        }

        private class AddNewUserUsecaseCallback : IUsecaseCallback<AddUserResponse>
        {
            private AddUserUseCase _useCase;

            public AddNewUserUsecaseCallback(AddUserUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(AddUserResponse response)
            {
                _useCase.PresenterCallback?.OnSuccess(response);
            }
        }

    }
}
