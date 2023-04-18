using Notes_Library.DataManager;
using Notes_Library.Domain.UseCase.CommentUseCases;
using Notes_Library.Model.BussinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;

namespace Notes_Library.Domain.UseCase.CommentUseCase
{

    public class AddReplyResponse
    {
        public ReplyBobj Reply;
    }
    public class AddReplyRequest
    {
        public int UserId { get; set; }
        public int RepliedTo { get; set; }
        public string ReplyText { get; set; }
        public int ParentId { get; set; }

    }
    public class AddReplyUseCase : UseCaseBase<AddReplyResponse>
    {
        private AddReplyRequest _request;
        private CommentsManager _manager;
        public AddReplyUseCase(AddReplyRequest request,IPresenterCallback<AddReplyResponse> callback):base(callback)
        {
            _request = request;
            _manager = CommentsManager.GetInstance;
        }

        public override void Action()
        {
            _manager.AddReply(_request.UserId, _request.RepliedTo, _request.ReplyText, _request.ParentId,new AddReplyUsecaseCallback(this));
        }

        public class AddReplyUsecaseCallback : IUsecaseCallback<AddReplyResponse>
        {
            private AddReplyUseCase _useCase;

            public AddReplyUsecaseCallback(AddReplyUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(AddReplyResponse response)
            {
                _useCase.PresenterCallback?.OnSuccess(response);
                    
            }
        }
    }

}
