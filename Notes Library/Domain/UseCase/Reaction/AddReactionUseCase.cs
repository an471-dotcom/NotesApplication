using Notes_Library.DataManager;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase.Reaction
{
    public class AddReactionResponse
    {
        public ReactionBobj Reaction { get; set; } 
    }

    public class AddReactionRequest
    {
        public int ReactedBy { get; set; }

        public int ReactedTo { get; set; }

        public Reactions ReactionType { get; set; }

    }


    public class AddReactionUseCase : UseCaseBase<AddReactionResponse>
    {
        private AddReactionRequest _request;

        private ReactionManager _manager;
        public AddReactionUseCase(AddReactionRequest request,IPresenterCallback<AddReactionResponse> callback):base(callback)
        {
            _request = request;
            _manager = ReactionManager.GetInstance;
        }

        public override void Action()
        {
            _manager.AddReaction(_request.ReactedBy, _request.ReactedTo, _request.ReactionType,new AddReactionUsecaseCallback(this));
        }

        private class AddReactionUsecaseCallback : IUsecaseCallback<AddReactionResponse>
        {
            private AddReactionUseCase _useCase;

            public AddReactionUsecaseCallback(AddReactionUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase?.PresenterCallback.OnError(error);
            }

            public void OnSuccess(AddReactionResponse response)
            {
               _useCase?.PresenterCallback.OnSuccess(response);
            }
        }
    }

}
