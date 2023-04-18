using Notes_Library.DataManager;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserActivities;

namespace Notes_Library.Domain.UseCase
{
    public class GetUserResponse
    {
        public UserInfo User { get; set; }
    }
    public class GetUserRequest
    {
        public int UserId { get; set; }

    }
    public class GetUserUseCase : UseCaseBase<GetUserResponse>
    {
        private GetUserRequest _request;
        private UserInfoManager _userManager;
        public GetUserUseCase(GetUserRequest request,IPresenterCallback<GetUserResponse> callback):base(callback)
        {
            _request= request;
            _userManager = UserInfoManager.GetInstance;
        }
        public override void Action()
        {
            _userManager.GetUser(_request.UserId, new GetUserUseCaseCallback(this));
        }

        private class GetUserUseCaseCallback:IUsecaseCallback<GetUserResponse>
        {
            private GetUserUseCase _useCase;

            public GetUserUseCaseCallback(GetUserUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);

            }

            public void OnSuccess(GetUserResponse response)
            {
              _useCase.PresenterCallback?.OnSuccess(response);
            }
        }
      
    }

}
