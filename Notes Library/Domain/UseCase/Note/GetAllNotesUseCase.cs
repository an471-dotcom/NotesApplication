using Notes_Library.DataManager;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain.UseCase
{
    public enum NoteType
    {
        [Description("All Notes")]
        AllNotes,

        [Description("Favorite Notes")]
        FavoriteNotes,

        [Description("Shared Notes")]
        SharedNotes,

        [Description("Notebook Notes")]
        NotebookNotes,

        [Description("Tagged Notes")]
        TaggedNotes,

    }
    public class GetAllNotesResponse
    {
        public List<NoteBobj> Notes { get; set; }
    }
    public class GetAllNotesRequest
    {
        public int UserId { get; set; }
    }

    public class GetAllNotesUseCase : UseCaseBase<GetAllNotesResponse>
    {
        private readonly GetAllNotesRequest _request;

        private readonly NotesManager _notesManager;
        public GetAllNotesUseCase(GetAllNotesRequest request, IPresenterCallback<GetAllNotesResponse> callback) : base(callback)
        {
            _request = request;

            _notesManager = NotesManager.GetInstance;
        }
        public override void Action()
        {
            _notesManager.GetNotes(_request.UserId, new GetAllNotesUsecaseCallback(this));

        }

        public class GetAllNotesUsecaseCallback : IUsecaseCallback<GetAllNotesResponse>
        {
            private GetAllNotesUseCase _usecase;

            public GetAllNotesUsecaseCallback(GetAllNotesUseCase usecase)
            {
                _usecase = usecase;
            }

            public void OnError(Exception error)
            {
                _usecase.PresenterCallback?.OnError(error);
            }

            public void OnSuccess(GetAllNotesResponse response)
            {
                _usecase.PresenterCallback?.OnSuccess(response);
            }
        }



    }
}
