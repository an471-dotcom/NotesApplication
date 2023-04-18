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
    public class CreateNoteResponse
    {
        public NoteBobj Note { get; set; }
    }

    public class CreateNoteRequest
    {
        public string NoteTitle { get; set; }
        public string NoteContent { get; set; }
        public int CreatedBy { get; set; }
        public string NoteBg { get; set; }

        public int? NotebookId { get; set; }

        public CancellationToken CancellationToken { get; set; }
        public IPresenterCallback<CreateNoteResponse> Callback { get; set; }
    }
  

    public class CreateNoteUseCase : UseCaseBase<CreateNoteResponse>
    {

        private readonly NotesManager _notesManager;
        private readonly CreateNoteRequest _request;
        public CreateNoteUseCase(CreateNoteRequest request,IPresenterCallback<CreateNoteResponse> callback) : base(callback)
        {
            _request= request;
            _notesManager= NotesManager.GetInstance;
        }
        public override void Action()
        {
            _notesManager.CreateNewNote(_request.NoteTitle, _request.NoteContent, _request.CreatedBy, _request.NoteBg, _request.NotebookId, new CreateNoteUseCaseCallback(this));
        }

        private class CreateNoteUseCaseCallback : IUsecaseCallback<CreateNoteResponse>
        {
            private CreateNoteUseCase _usecase;

            public CreateNoteUseCaseCallback(CreateNoteUseCase usecase)
            {
                _usecase = usecase;
            }

            public void OnError(Exception error)
            {
                _usecase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(CreateNoteResponse response)
            {
               _usecase.PresenterCallback?.OnSuccess(response);
            }
        }
       
    }
}
