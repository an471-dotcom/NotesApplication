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
    public class UpdateUserResponse
    {
        public UserInfo UpdatedUser { get; set; }
    }
    public class UpdateUserRequest
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DefaultNoteColor { get; set; }

        
    }

    public class UpdateUserUseCase : UseCaseBase<UpdateUserResponse>
    {
        private UpdateUserRequest _request;
        private UserInfoManager _manager;
        public UpdateUserUseCase(UpdateUserRequest request,IPresenterCallback<UpdateUserResponse> callback):base(callback)
        {
            _request = request;
            _manager = UserInfoManager.GetInstance;
        }

        public override void Action()
        {
            _manager.UpdateUser(_request.UserId, _request.UserName, _request.Password, _request.DefaultNoteColor,new UpdateUserUsecaseCallback(this));
        }

        private class UpdateUserUsecaseCallback:IUsecaseCallback<UpdateUserResponse>
        {
            private UpdateUserUseCase _useCase;

            public UpdateUserUsecaseCallback(UpdateUserUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(UpdateUserResponse response)
            {
                _useCase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
}
