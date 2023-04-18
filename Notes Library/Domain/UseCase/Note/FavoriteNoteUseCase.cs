using Notes_Library.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase
{
    public class FavoriteNoteResponse
    {
        public int NoteId { get; set; }
    }
    public class FavoriteNoteRequest
    {
        public int UserId { get; set; }
        public int NoteId { get; set; }

    }
    public class FavoriteNoteUseCase : UseCaseBase<FavoriteNoteResponse>
    {
        private readonly FavoriteNoteRequest _request;

        private readonly NotesManager _notesManager;
        public FavoriteNoteUseCase(FavoriteNoteRequest request,IPresenterCallback<FavoriteNoteResponse> callback) : base(callback)
        {
            _request = request;

            _notesManager = NotesManager.GetInstance;
        }
        public override void Action()
        {
            _notesManager.MakeFavorite(_request.UserId, _request.NoteId, new FavoriteNoteUseCaseCallback(this));
        }

        private class FavoriteNoteUseCaseCallback : IUsecaseCallback<FavoriteNoteResponse>
        {
            private FavoriteNoteUseCase _useCase;

            public FavoriteNoteUseCaseCallback(FavoriteNoteUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
               _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(FavoriteNoteResponse response)
            {
               _useCase.PresenterCallback?.OnSuccess(response);
            }
        }
       
    }
}
