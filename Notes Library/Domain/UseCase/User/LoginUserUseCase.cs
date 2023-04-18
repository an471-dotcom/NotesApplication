using Notes_Library.DataManager;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase
{
    
    public class LoginUserResponse
    {
        public UserInfo User { get; set; }
    }
    public class LoginUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserUseCase : UseCaseBase<LoginUserResponse>
    {
        private LoginUserRequest _request;
        private UserInfoManager _userManager;
        public LoginUserUseCase(LoginUserRequest request,IPresenterCallback<LoginUserResponse> callback):base(callback)
        {
            _request = request;
            _userManager = UserInfoManager.GetInstance;
        }
        public override void Action()
        {
            _userManager.LoginUser(_request.Email, _request.Password, new LoginUserUsecaseCallback(this));
        }

        private class LoginUserUsecaseCallback: IUsecaseCallback<LoginUserResponse>
        {
            private LoginUserUseCase _useCase;

            public LoginUserUsecaseCallback(LoginUserUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(LoginUserResponse response)
            {
                _useCase.PresenterCallback?.OnSuccess(response);
            }
        }
       
    }
}
