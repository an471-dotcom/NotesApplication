using Notes_Library.Data.DataAdapter.Contracts;
using Notes_Library.Data.DataAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes_Library.Models;
using Windows.UI.WindowManagement;
using Notes_Library.Data.Interface;

namespace Notes_Library.Data.DataHandler
{
    internal class NotebookDBHandler:INotebookDBHandler
    {
        private readonly IDatabaseAdapter _db;
        private NotebookDBHandler()
        {
            _db = new DatabaseAdapter();
        }
        private static NotebookDBHandler _instance = null;

        public static NotebookDBHandler GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NotebookDBHandler();
                }
                return _instance;
            }
        }
        public async Task<NoteBook> AddNotebook(NoteBook notebook)
        {  
            await _db.InsertAsync(notebook);
            return notebook;
        }

        public async Task<List<NoteBook>> GetUserNotebooks(int userId)
        {
            return (await _db.GetTableAsync<NoteBook>()).Where(nb => nb.CreatedBy == userId).ToList();
        }

        public async Task<NoteBook> UpdateNotebook(NoteBook notebook)
        {
            await _db.UpdateAsync(notebook);
            return notebook;
        }

        public async Task DeleteNotebook(int notebookId)
        {
            var notebook = (await _db.GetTableAsync<NoteBook>()).FirstOrDefault(nb => nb.NotebookId == notebookId);

            await _db.DeleteAsync<NoteBook>(notebook.NotebookId);
        }

        public async Task<NoteBook> GetNotebookById(int notebookId)
        {
            return (await _db.GetTableAsync<NoteBook>()).FirstOrDefault(nb => nb.NotebookId == notebookId);
        }

        public async Task<string> GetNotebookName(int notebookId)
        {
            return (await _db.GetTableAsync<NoteBook>()).FirstOrDefault(nb => nb.NotebookId == notebookId).Name;
        }
    }
}
