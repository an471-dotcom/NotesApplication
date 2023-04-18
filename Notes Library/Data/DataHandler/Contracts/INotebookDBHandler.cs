using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Data.Interface
{
    internal interface INotebookDBHandler
    {
        Task<NoteBook> AddNotebook(NoteBook notebook);

        Task<List<NoteBook>> GetUserNotebooks(int userId);

        Task<NoteBook> UpdateNotebook(NoteBook notebook);

        Task DeleteNotebook(int notebookId);

        Task<NoteBook> GetNotebookById(int notebookId);

        Task<string> GetNotebookName(int notebookId);
    }
}
