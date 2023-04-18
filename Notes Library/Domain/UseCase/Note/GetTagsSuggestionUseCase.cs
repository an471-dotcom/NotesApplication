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

    public class GetTagsSuggestionResponse
    {
        public List<Tag> Tags { get; set; }

    }
    public class GetTagsSuggestionRequest
    {
        public string input { get; set; }
        public int UserId { get; set; }

    }
    public class GetTagsSuggestionUseCase : UseCaseBase<GetTagsSuggestionResponse>
    {
        private GetTagsSuggestionRequest _request;

        private TagsManager _manager;
        public GetTagsSuggestionUseCase(GetTagsSuggestionRequest request, IPresenterCallback<GetTagsSuggestionResponse> callback):base(callback)
        {
            _request = request;
            _manager = TagsManager.GetInstance;
        }
        public override void Action()
        {
            _manager.GetNoteTagsSuggestion(_request.input, _request.UserId,new GetTagsSuggestionPresenterCallback(this));
        }

        private class GetTagsSuggestionPresenterCallback : IUsecaseCallback<GetTagsSuggestionResponse>
        {
            private GetTagsSuggestionUseCase _usecase;

            public GetTagsSuggestionPresenterCallback(GetTagsSuggestionUseCase usecase)
            {
                _usecase = usecase;
            }

            public void OnError(Exception error)
            {
                _usecase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(GetTagsSuggestionResponse response)
            {
                _usecase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
    
}
