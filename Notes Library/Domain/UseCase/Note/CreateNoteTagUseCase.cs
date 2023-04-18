using Notes_Library.DataManager;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Media.Casting;

namespace Notes_Library.Domain.UseCase
{
    public class CreateNoteTagResponse
    {
        public Tag Tag { get; set; }
    }
    public class CreateNoteTagRequest
    {
        public string TagName { get; set; }

        public int NoteId { get; set; }

        public int UserId { get; set; }

        public string TagColor { get; set; }
    }
    public class CreateNoteTagUseCase : UseCaseBase<CreateNoteTagResponse>
    {
        private CreateNoteTagRequest _request;

        private TagsManager _manager;
        public CreateNoteTagUseCase(CreateNoteTagRequest request,IPresenterCallback<CreateNoteTagResponse> callback):base(callback)
        {
            _request = request;

            _manager = TagsManager.GetInstance;
        }
        public override void Action()
        {
            _manager.CreateNoteTag(_request.TagName, _request.UserId,_request.NoteId, _request.TagColor,new CreateNoteTagUseCaseCallback(this));
        }

        private class CreateNoteTagUseCaseCallback : IUsecaseCallback<CreateNoteTagResponse>
        {
            private CreateNoteTagUseCase _usecase;

            public CreateNoteTagUseCaseCallback(CreateNoteTagUseCase usecase)
            {
                _usecase = usecase;
            }

            public void OnError(Exception error)
            {
                _usecase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(CreateNoteTagResponse response)
            {
                _usecase.PresenterCallback?.OnSuccess(response);
            }
        }
        
    }
    
}
