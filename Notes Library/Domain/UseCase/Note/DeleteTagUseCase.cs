using Notes_Library.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase.Note
{
    public class DeleteTagResponse
    {
        public int DeletedTagId;

    }
    public class DeleteTagResquest
    {
        public int TagId;
    }
    public class DeleteTagUseCase : UseCaseBase<DeleteTagResponse>
    {
        private DeleteTagResquest _request;

        private TagsManager _manager;
        public DeleteTagUseCase(DeleteTagResquest request,IPresenterCallback<DeleteTagResponse> callback):base(callback) 
        {
            _request = request;
            _manager = TagsManager.GetInstance;
        }

        public override void Action()
        {
            _manager.DeleteTag(_request.TagId, new DeleteTagUsecaseCallback(this));
        }

        private class DeleteTagUsecaseCallback:IUsecaseCallback<DeleteTagResponse>
        {
            private DeleteTagUseCase _useCase;

            public DeleteTagUsecaseCallback(DeleteTagUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(DeleteTagResponse response)
            {
               _useCase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
}
