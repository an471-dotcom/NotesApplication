using Notes_Library.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase
{
    public class DeleteNoteTagResponse
    {
       public int TagId { get; set; }
    }

    public class DeleteNoteTagRequest
    {
        public int NoteId { get; set; }
        public int TagId { get; set; }

    }

    public class DeleteNoteTagUseCase : UseCaseBase<DeleteNoteTagResponse>
    {
        private DeleteNoteTagRequest _request;

        private TagsManager _manager;

        public DeleteNoteTagUseCase(DeleteNoteTagRequest request, IPresenterCallback<DeleteNoteTagResponse> callback):base(callback)
        {
            _request = request;
            _manager = TagsManager.GetInstance;
        }
        public override void Action()
        {
            _manager.DeleteNoteTag(_request.NoteId, _request.TagId, new DeleteNoteTagUseCaseCallback(this));

        }

        private class DeleteNoteTagUseCaseCallback: IUsecaseCallback<DeleteNoteTagResponse>
        {
            private DeleteNoteTagUseCase _useCase;

            public DeleteNoteTagUseCaseCallback(DeleteNoteTagUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
               _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(DeleteNoteTagResponse response)
            {
               _useCase.PresenterCallback?.OnSuccess(response);
            }

        }

        
    }
}
