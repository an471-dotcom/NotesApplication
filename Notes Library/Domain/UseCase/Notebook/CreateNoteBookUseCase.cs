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
    public class CreateNotebookResponse
    {
        public NoteBookBobj Notebook { get; set; }
    }
    public class CreateNotebookRequest
    {
        public string Title { get; set; }
        public int UserId { get; set; }
        public string NoteBookCover { get; set; }
    }
    public class CreateNoteBookUseCase : UseCaseBase<CreateNotebookResponse>
    {
        private CreateNotebookRequest _request;

        private NotebookManager _notebookManager;
        public CreateNoteBookUseCase(CreateNotebookRequest request,IPresenterCallback<CreateNotebookResponse> callback):base(callback)
        {
           _request = request;
            _notebookManager = NotebookManager.GetInstance;

        }

        public override void Action()
        {
            _notebookManager.CreateNotebook(_request.Title, _request.UserId,_request.NoteBookCover, new CreateNoteBookUseCaseCallback(this));
        }
        public class CreateNoteBookUseCaseCallback: IUsecaseCallback<CreateNotebookResponse>
        {
            private CreateNoteBookUseCase _useCase;

            public CreateNoteBookUseCaseCallback(CreateNoteBookUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(CreateNotebookResponse response)
            {
                _useCase.PresenterCallback?.OnSuccess(response);
            }

        }


       
    }
}
