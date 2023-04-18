using Notes_Library.DataManager;
using Notes_Library.Model.BussinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase
{
    public class UnshareNoteResponse
    {
        public int UserId { get; set; }

        public int NoteId { get; set; }
        public IEnumerable<CommentBobj> Comments { get; set; }
    }

    public class UnshareNoteRequest
    {
        public int NoteId { get; set; }
        public int UserId { get; set; }

    }
    public class UnshareNoteUseCase : UseCaseBase<UnshareNoteResponse>
    {
        private UnshareNoteRequest _request;
        private SharedNoteManager _manager;
        public UnshareNoteUseCase(UnshareNoteRequest request,IPresenterCallback<UnshareNoteResponse> callback) : base(callback)
        {
            _request = request;
            _manager = SharedNoteManager.GetInstance;
        }

        public override void Action()
        {
            _manager.UnshareNote(_request.UserId, _request.NoteId,new UnshareNoteUsecaseCallback(this));
        }

        private class UnshareNoteUsecaseCallback : IUsecaseCallback<UnshareNoteResponse>
        {
            private UnshareNoteUseCase _usecase;

            public UnshareNoteUsecaseCallback(UnshareNoteUseCase usecase)
            {
                _usecase = usecase;
            }

            public void OnError(Exception error)
            {
                _usecase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(UnshareNoteResponse response)
            {
                _usecase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
}
