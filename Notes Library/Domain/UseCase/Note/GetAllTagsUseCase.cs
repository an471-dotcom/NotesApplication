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
    public class GetAllTagsResponse
    {
        public List<Tag> Tags { get; set; }
    }
    public class GetAllTagsRequest
    {
        public int UserId { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public IPresenterCallback<GetAllTagsResponse> Callback { get; set; }
    }
    public class GetAllTagsUseCase : UseCaseBase<GetAllTagsResponse>
    {
        private GetAllTagsRequest _request;
        private TagsManager _manager;
        public GetAllTagsUseCase(GetAllTagsRequest request,IPresenterCallback<GetAllTagsResponse> callback) : base(callback)
        {
            _request = request;
            _manager = TagsManager.GetInstance;
        }
        public override void Action()
        {
            _manager.GetAllTags(_request.UserId, new GetAllTagsUseCaseCallback(this));
        }
        private class GetAllTagsUseCaseCallback:IUsecaseCallback<GetAllTagsResponse>
        {
            private GetAllTagsUseCase _useCase;

            public GetAllTagsUseCaseCallback(GetAllTagsUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(GetAllTagsResponse response)
            {
               _useCase.PresenterCallback?.OnSuccess(response);
            }
        }

     
    }
}
