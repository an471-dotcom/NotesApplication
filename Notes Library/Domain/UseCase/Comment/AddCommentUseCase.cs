using Notes_Library.DataManager;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase.CommentUseCases
{
    public class AddCommentResponse
    {
        public CommentBobj Comment { get; set; }
    }
    public class AddCommentRequest
    {
        public int UserId { get; set; }
        public int CommentedTo { get; set; }
        public string CommentText { get; set; }
        public int? ParentId { get; set; }

      
    }


    public class AddCommentUseCase : UseCaseBase<AddCommentResponse>
    {
        private AddCommentRequest _request;
        private CommentsManager _manager;

        public AddCommentUseCase(AddCommentRequest request,IPresenterCallback<AddCommentResponse> callback):base(callback) 
        {
            _request = request;
            _manager = CommentsManager.GetInstance;
        }

        public override void Action()
        {
            _manager.AddComment(_request.UserId, _request.CommentedTo, _request.CommentText, _request.ParentId, new AddCommentUsecaseCallback(this));
        }

        private class AddCommentUsecaseCallback : IUsecaseCallback<AddCommentResponse>
        {
            private AddCommentUseCase _useCase;

            public AddCommentUsecaseCallback(AddCommentUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(AddCommentResponse response)
            {
                _useCase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
}
