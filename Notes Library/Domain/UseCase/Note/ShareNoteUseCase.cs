using Notes_Library.DataManager;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase
{
    public enum ShareNoteStatus
    {
        UserNotFound,
        NoteAlreadyShared,
        NoteShared

    }
    public class ShareNoteResponse
    {
        public List<UserInfo> SharedList { get; set; }

    }
    public class ShareNoteRequest
    {
        public IEnumerable<string> EmailList { get; set; }
        public int NoteId { get; set; }

    }
    public class ShareNoteUseCase : UseCaseBase<ShareNoteResponse>
    {
        private ShareNoteRequest _request;
        private SharedNoteManager _manager;
        public ShareNoteUseCase(ShareNoteRequest request,IPresenterCallback<ShareNoteResponse> callback) : base(callback)
        {
            _request= request;
            _manager = SharedNoteManager.GetInstance;
        }
        public override void Action()
        {
            _manager.ShareNote(_request.EmailList,_request.NoteId, new ShareNoteUseCaseCallback(this) );
        }
        private class ShareNoteUseCaseCallback : IUsecaseCallback<ShareNoteResponse>
        {
            private ShareNoteUseCase _useCase;

            public ShareNoteUseCaseCallback(ShareNoteUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
               _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(ShareNoteResponse response)
            {
               _useCase.PresenterCallback?.OnSuccess(response);
            }
        }

       
    }
}
