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
    public class GetNotesByTagResponse
    {
        public string TagName { get; set; }
        public  List<NoteBobj> Notes { get; set; }
    }
    public class GetNotesByTagRequest
    {
        public int TagId { get; set; }

    }
    public class GetNotesByTagUseCase : UseCaseBase<GetNotesByTagResponse>
    {
        private GetNotesByTagRequest _request;
        private NotesManager _manager;
        public GetNotesByTagUseCase(GetNotesByTagRequest request,IPresenterCallback<GetNotesByTagResponse> callback):base(callback)
        {
            _request = request;
            _manager = NotesManager.GetInstance;
        }

        public override void Action()
        {
            _manager.GetNotesWithTag(_request.TagId,new GetNotesByTagUsecaseCallback(this));
        }

        private class GetNotesByTagUsecaseCallback : IUsecaseCallback<GetNotesByTagResponse>
        {
            private GetNotesByTagUseCase _useCase;

            public GetNotesByTagUsecaseCallback(GetNotesByTagUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(GetNotesByTagResponse response)
            {
                _useCase.PresenterCallback?.OnSuccess(response);
            }
        }
    }

   
}
