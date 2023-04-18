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
    public class GetUserSuggestionResponse
    {
        public List<UserInfo> UserEmails { get; set; }
       // public List<String> UserEmails { get; set; }
    }
    public class GetUserSuggestionRequest
    {
        public string UserInput { get; set; }
        public int NoteId { get; set; }
        public int UserId { get; set; }
        
    }

    public class GetUserSuggestionUseCase : UseCaseBase<GetUserSuggestionResponse>
    {
        private readonly GetUserSuggestionRequest _request;
        private readonly NotesManager _manager;
        public GetUserSuggestionUseCase(GetUserSuggestionRequest request,IPresenterCallback<GetUserSuggestionResponse> callback):base(callback)
        {
            _request  = request;
            _manager = NotesManager.GetInstance;
        }
        public override void Action()
        {
            
            _manager.GetUserSuggestion(_request.UserId,_request.NoteId, new GetUserSuggestionUseCaseCallback(this));
        }

        public class GetUserSuggestionUseCaseCallback:IUsecaseCallback<GetUserSuggestionResponse>
        {
            private GetUserSuggestionUseCase _useCase;

            public GetUserSuggestionUseCaseCallback(GetUserSuggestionUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(GetUserSuggestionResponse response)
            {
                _useCase.PresenterCallback?.OnSuccess(response);
            }
        }
       
    }

}
