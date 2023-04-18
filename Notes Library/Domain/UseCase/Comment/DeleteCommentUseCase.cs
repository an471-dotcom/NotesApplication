using Notes_Library.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase.CommentUseCase
{

    public class DeleteCommentResponse
    { 
        public int DeletedCommentId { get; set; }
    }

    public class DeleteCommentRequest
    {
        public int CommentId { get; set; }

      
    }


    public class DeleteCommentUseCase : UseCaseBase<DeleteCommentResponse>
    {
        private DeleteCommentRequest _request;

        private CommentsManager _manager;

        public DeleteCommentUseCase(DeleteCommentRequest request,IPresenterCallback<DeleteCommentResponse> callback):base(callback) 
        {
            _request = request;
            _manager = CommentsManager.GetInstance;
        }

        public override void Action()
        {
            _manager.DeleteComment(_request.CommentId,new DeleteCommentUsecaseCallback(this));
        }
        private class DeleteCommentUsecaseCallback:IUsecaseCallback<DeleteCommentResponse>
        {
            private DeleteCommentUseCase _usecase;

            public DeleteCommentUsecaseCallback(DeleteCommentUseCase usecase)
            {
                _usecase = usecase;
            }

            public void OnError(Exception error)
            {
                _usecase?.PresenterCallback.OnError(error);
            }

            public void OnSuccess(DeleteCommentResponse response)
            {
               _usecase?.PresenterCallback.OnSuccess(response);
            }
        }

    }
}
