using Notes_Library.DataManager;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase
{
    public class GetNoteBooksResponse
    {
        public List<NoteBookBobj> Notebooks { get; set; }
    }

    public class GetNotebooksRequest
    {
        public int UserId { get; set; }

    }
    public class GetNoteBooksUseCase : UseCaseBase<GetNoteBooksResponse>
    {
        private GetNotebooksRequest _request;
        private NotebookManager _notebookManager;
        public GetNoteBooksUseCase(GetNotebooksRequest request,IPresenterCallback<GetNoteBooksResponse> callback) : base(callback)
        {
            _request = request;
            _notebookManager = NotebookManager.GetInstance;
        }
        public override void Action()
        {
            _notebookManager.GetNotebooks(_request.UserId, new GetNoteBooksUseCaseCallback(this));
        }

        private class GetNoteBooksUseCaseCallback:IUsecaseCallback<GetNoteBooksResponse>
        {
            private GetNoteBooksUseCase _useCase;

            public GetNoteBooksUseCaseCallback(GetNoteBooksUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(GetNoteBooksResponse response)
            {
              _useCase.PresenterCallback.OnSuccess(response);
            }
        }

        
    }
}
