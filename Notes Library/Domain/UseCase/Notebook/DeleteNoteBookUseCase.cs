using Notes_Library.DataManager;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments.DataProvider;

namespace Notes_Library.Domain.UseCase
{
    public class DeleteNoteBookResponse
    {
        public int NotebookId; 
    }
    public class DeleteNotebookRequest
    {
        public int NotebookId { get; set; }

       
    }
    public class DeleteNoteBookUseCase : UseCaseBase<DeleteNoteBookResponse>
    {
        private DeleteNotebookRequest _request;

        private NotebookManager _notebookManager;
        public DeleteNoteBookUseCase(DeleteNotebookRequest request, IPresenterCallback<DeleteNoteBookResponse> callback) : base(callback)
        {
            _request = request;
            _notebookManager = NotebookManager.GetInstance;
        }
        public override void Action()
        {
            _notebookManager.DeleteNotebook(_request.NotebookId, new DeleteNoteBookUseCaseCallback(this));
        }

        private class DeleteNoteBookUseCaseCallback:IUsecaseCallback<DeleteNoteBookResponse>
        {

            private DeleteNoteBookUseCase _useCase;

            public DeleteNoteBookUseCaseCallback(DeleteNoteBookUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(DeleteNoteBookResponse response)
            {
                _useCase.PresenterCallback?.OnSuccess(response);
            }
        }

        
    }
}
