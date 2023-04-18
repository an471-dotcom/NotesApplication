using Notes_Library.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase
{
    public class DeleteReplyResponse
    {
        public int DeletedReplyParentId;
        public int DeletedReplyId;
    }

    public class DeleteReplyRequest
    {
        public int ReplyId;

    }
    public class DeleteReplyUseCase : UseCaseBase<DeleteReplyResponse>
    {
        private DeleteReplyRequest _request;
        private CommentsManager _manager;

        public DeleteReplyUseCase(DeleteReplyRequest request, IPresenterCallback<DeleteReplyResponse> callback) : base(callback)
        {
            _request = request;
            _manager = CommentsManager.GetInstance;
        }

        public override void Action()
        {
            _manager.DeleteReply(_request.ReplyId, new DeleteReplyUsecaseCallback(this));
        }
        private class DeleteReplyUsecaseCallback : IUsecaseCallback<DeleteReplyResponse>
        {
            private DeleteReplyUseCase _usecase;

            public DeleteReplyUsecaseCallback(DeleteReplyUseCase usecase)
            {
                _usecase = usecase;
            }

            public void OnError(Exception error)
            {
                _usecase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(DeleteReplyResponse response)
            {
                _usecase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
}
