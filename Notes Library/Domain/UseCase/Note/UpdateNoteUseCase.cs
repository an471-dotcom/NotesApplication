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
    public class UpdateNoteRequest
    {
       
        public int NoteId { get; set; }
        public string NoteTitle { get; set; }
        public string NoteContent { get; set; }
        public string NoteBg { get; set; }
    }
    public class UpdateNoteResponse
    {
        public NoteBobj Note { get; set; }
    }
    
    
    public class UpdateNoteUseCase : UseCaseBase<UpdateNoteResponse>
    {
        private readonly UpdateNoteRequest _request;
        private readonly NotesManager _notesManager;
        public UpdateNoteUseCase(UpdateNoteRequest request,IPresenterCallback<UpdateNoteResponse> callback) : base(callback)
        {
            _request= request;
            _notesManager = NotesManager.GetInstance;
        }

        public override void Action()
        {
            _notesManager.UpdateNote(_request.NoteId, _request.NoteTitle, _request.NoteContent, _request.NoteBg, new UpdateNoteUseCaseCallback(this));
        }

        private class UpdateNoteUseCaseCallback : IUsecaseCallback<UpdateNoteResponse>
        {
            private UpdateNoteUseCase _useCase;

            public UpdateNoteUseCaseCallback(UpdateNoteUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(UpdateNoteResponse response)
            {
                _useCase.PresenterCallback?.OnSuccess(response);
            }
        }

       
    }
}
