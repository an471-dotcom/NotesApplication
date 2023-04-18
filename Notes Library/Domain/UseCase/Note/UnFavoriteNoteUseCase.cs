using Notes_Library.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase
{
    public class UnFavoriteNoteResponse
    {
        public int NoteId;

    }
    public class UnFavoriteRequest
    {
        public int UserId { get; set; }

        public int NoteId { get; set; }

    }
    public class UnFavoriteNoteUseCase : UseCaseBase<UnFavoriteNoteResponse>
    {
        private readonly UnFavoriteRequest _request;
        private readonly NotesManager _notesManager;
        public UnFavoriteNoteUseCase(UnFavoriteRequest request, IPresenterCallback<UnFavoriteNoteResponse> callback) : base(callback)
        {
            _request = request;
            _notesManager = NotesManager.GetInstance;
        }
        public override void Action()
        {
            _notesManager.RemoveFavorite(_request.UserId, _request.NoteId, new UnFavoriteNoteUseCaseCallback(this));
        }

        private class UnFavoriteNoteUseCaseCallback : IUsecaseCallback<UnFavoriteNoteResponse>
        {
            private UnFavoriteNoteUseCase _useCase;

            public UnFavoriteNoteUseCaseCallback(UnFavoriteNoteUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnError(Exception error)
            {
                _useCase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(UnFavoriteNoteResponse response)
            {
                _useCase.PresenterCallback?.OnSuccess(response);
            }
        }

    }
}
