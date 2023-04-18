using Notes_Library.DataManager;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase.Note
{
    public class UpdateTagRequest
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public string TagColor { get; set; }
    }

    public class UpdateTagResponse
    {
        public Tag UpdatedTag;
    }
    public class UpdateTagUseCase:UseCaseBase<UpdateTagResponse>
    {
        private UpdateTagRequest _request;
        private TagsManager _manager;

        public UpdateTagUseCase(UpdateTagRequest request,IPresenterCallback<UpdateTagResponse> callback) : base(callback)
        {
            _request = request;
            _manager = TagsManager.GetInstance;
        }

        public override void Action()
        {
            _manager.UpdateTag(_request.TagId,_request.TagName,_request.TagColor,new UpdateTagUseCaseCallback(this));
        }
        private class UpdateTagUseCaseCallback : IUsecaseCallback<UpdateTagResponse>
        {
            private UpdateTagUseCase _useCase;
            public UpdateTagUseCaseCallback(UpdateTagUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback.OnError(error);
            }

            public void OnSuccess(UpdateTagResponse response)
            {
                _useCase.PresenterCallback.OnSuccess(response);
            }
        }
    }
}
