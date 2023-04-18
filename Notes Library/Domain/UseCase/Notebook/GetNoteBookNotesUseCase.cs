using Notes_Library.DataManager;
using Notes_Library.Model.BussinessObjects;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Notes_Library.Domain.UseCase
{
    public class GetNotebookNotesResponse
    {
        public string NotebookTitle;

        public List<NoteBobj> Notes;
    }
    public class GetNotebookNotesRequest
    {
        public int NotebookId;

    }
    public class GetNoteBookNotesUseCase : UseCaseBase<GetNotebookNotesResponse>
    {
        private GetNotebookNotesRequest _request;

        private NotesManager _notesManager;
        public GetNoteBookNotesUseCase(GetNotebookNotesRequest request,IPresenterCallback<GetNotebookNotesResponse> callback) : base(callback)
        {
            _request = request;

            _notesManager = NotesManager.GetInstance;
        }
        public override void Action()
        {
            _notesManager.GetNotebookNotes(_request.NotebookId, new GetNoteBookNotesUseCaseCallback(this));
        }

        private class GetNoteBookNotesUseCaseCallback:IUsecaseCallback<GetNotebookNotesResponse>
        {
            private GetNoteBookNotesUseCase _useCase;

            public GetNoteBookNotesUseCaseCallback(GetNoteBookNotesUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
               _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(GetNotebookNotesResponse response)
            {
              _useCase.PresenterCallback?.OnSuccess(response);
            }
        }

        
    }
}
