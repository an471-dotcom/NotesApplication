using Notes_Library.Data.DataHandler;
using Notes_Library.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase.Notebook
{

    public class ChangeNoteBookCoverRequest
    {
        public int NoteBookId { get; set;}

        public string NoteBookCover { get; set; }
    }

    public class ChangeNoteBookCoverResponse
    {
        public int NoteBookId { get; set; }
        public string NoteBookCover { get; set;}
    }
    public class ChangeNoteBookCover:UseCaseBase<ChangeNoteBookCoverResponse>
    {
        private ChangeNoteBookCoverRequest _request;

        private NotebookManager _manager;

        public ChangeNoteBookCover(ChangeNoteBookCoverRequest request,IPresenterCallback<ChangeNoteBookCoverResponse> callback):base(callback)
        {
            _request = request;

            _manager = NotebookManager.GetInstance;
        }

        public override void Action()
        {
            _manager.ChangeNoteBookCover(_request.NoteBookId, _request.NoteBookCover,new ChangeNoteBookCoverUseCaseCallback(this));
        }

        private class ChangeNoteBookCoverUseCaseCallback : IUsecaseCallback<ChangeNoteBookCoverResponse>
        {
            private ChangeNoteBookCover _useCase;

            public ChangeNoteBookCoverUseCaseCallback(ChangeNoteBookCover useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(ChangeNoteBookCoverResponse response)
            {
               _useCase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
    
    
}
