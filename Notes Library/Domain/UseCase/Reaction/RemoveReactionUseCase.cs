using Notes_Library.DataManager;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase.Reaction
{

    public class RemoveReactionResponse
    {
        public int RemovedReactionId { get; set; }
    }

    public class RemoveReactionRequest
    {
        public int ReactionId { get; set; }

    }
    public class RemoveReactionUseCase : UseCaseBase<RemoveReactionResponse>
    {
        private RemoveReactionRequest _request;
        private ReactionManager _manager;

        public RemoveReactionUseCase(RemoveReactionRequest request,IPresenterCallback<RemoveReactionResponse> callback):base(callback)
        {
            _request = request;
            _manager = ReactionManager.GetInstance;
        }

        public override void Action()
        {
            _manager.RemoveReaction(_request.ReactionId,new RemoveReactionUsecaseCallback(this));
        }

        private class RemoveReactionUsecaseCallback : IUsecaseCallback<RemoveReactionResponse>
        {
            private RemoveReactionUseCase _usecase;

            public RemoveReactionUsecaseCallback(RemoveReactionUseCase usecase)
            {
                _usecase = usecase;
            }

            public void OnError(Exception error)
            {
                _usecase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(RemoveReactionResponse response)
            {
                _usecase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
}
