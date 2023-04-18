using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes_Library.Data.DataAdapter.Contracts
{
    public interface IDatabaseAdapter
    {
        Task<List<T>> GetTableAsync<T>() where T : new();

        Task<int> InsertAsync<T>(T obj) where T : new();

        Task<int> DeleteAsync<T>(int id) where T : new();

        Task<int> UpdateAsync<T>(T obj) where T : new();

        Task<int> DeleteAsync<T>(T obj) where T : new();



    }
}
