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
    public class RenameNoteBookResponse
    {
        public NoteBookBobj UpdatedNotebook;
    }
    public class RenameNoteBookRequest
    {
        public string Title { get; set; }
        public int NoteBookId { get; set; }
       
    }

    public class RenameNoteBookUseCase : UseCaseBase<RenameNoteBookResponse>
    {
        private RenameNoteBookRequest _request;

        private NotebookManager _notebookManager;

        public RenameNoteBookUseCase(RenameNoteBookRequest request,IPresenterCallback<RenameNoteBookResponse> callback):base(callback)
        {
            _request= request;
            _notebookManager = NotebookManager.GetInstance;
        }

        public override void Action()
        {
            _notebookManager.RenameNotebook(_request.Title, _request.NoteBookId, new RenameNoteBookUseCaseCallback(this));
        }
        
        private class RenameNoteBookUseCaseCallback:IUsecaseCallback<RenameNoteBookResponse>
        {
            private RenameNoteBookUseCase _useCase;

            public RenameNoteBookUseCaseCallback(RenameNoteBookUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
               _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(RenameNoteBookResponse response)
            {
               _useCase.PresenterCallback?.OnSuccess(response);
            }
        }
        
    }
}
