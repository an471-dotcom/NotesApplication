using Notes_Library.Data.DataAdapter.Contracts;
using Notes_Library.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Windows.Storage;

namespace Notes_Library.Data.DataAdapter
{
    public class DatabaseAdapter : IDatabaseAdapter
    {
        public static SQLiteAsyncConnection db;

        public static async Task Init()
        {
            if (db != null)
                return;
            await ApplicationData.Current.LocalFolder.CreateFileAsync("NotesDatabase.db", CreationCollisionOption.OpenIfExists);
            var databasePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "NotesDatabase.db");
            var option = new SQLiteConnectionString(databasePath, true, "password");
            db = new SQLiteAsyncConnection(option);
            await db.CreateTableAsync<Favorite>();
            await db.CreateTableAsync<NoteBook>();
            await db.CreateTableAsync<Note>();
            await db.CreateTableAsync<SharedNotes>();
            await db.CreateTableAsync<UserInfo>();
            await db.CreateTableAsync<Tag>();
            await db.CreateTableAsync<TaggedNotes>();
            await db.CreateTableAsync<Comment>();
            await db.CreateTableAsync<Reaction>();
        }

        public async Task<List<T>> GetTableAsync<T>() where T : new()
        {
            await Init();
            return await db.Table<T>().ToListAsync();
        }
        public async Task<int> InsertAsync<T>(T obj) where T : new()
        {
            await Init();
            return await db.InsertAsync(obj);
        }

        public async Task<int> DeleteAsync<T>(int id) where T : new()
        {
            await Init();
            return await db.DeleteAsync<T>(id);

        }

        public async Task<int> DeleteAsync<T>(T obj) where T:new() 
        {
            await Init();
            return await db?.DeleteAsync(obj);
       }

        public async Task<int> UpdateAsync<T>(T record) where T : new()
        {
            await Init();
            return await db.UpdateAsync(record);
        }

        
    }
}
