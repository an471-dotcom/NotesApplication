using Notes_Library.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase
{
    public class DeleteNoteResponse
    {
        public int Result { get; set; }
        public int NoteId { get; set; }
    }
    public class DeleteNoteRequest
    {
        public int UserId { get; set; }
        public int NoteId { get; set; }

    }
    public class DeleteNoteUseCase : UseCaseBase<DeleteNoteResponse>
    {
        private readonly DeleteNoteRequest _request;

        private readonly NotesManager _notesManager;
        public DeleteNoteUseCase(DeleteNoteRequest request,IPresenterCallback<DeleteNoteResponse> callback):base(callback)
        {
            _request = request;
            _notesManager = NotesManager.GetInstance;

        }
        public override void Action()
        {
           _notesManager.DeleteNote(_request.UserId, _request.NoteId, new DeleteNoteUseCaseCallback(this));
        }

        private class DeleteNoteUseCaseCallback: IUsecaseCallback<DeleteNoteResponse>
        {
            private DeleteNoteUseCase _useCase;

            public DeleteNoteUseCaseCallback(DeleteNoteUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
               _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(DeleteNoteResponse response)
            {
              _useCase.PresenterCallback?.OnSuccess(response);
            }
        }
        
    }
}
