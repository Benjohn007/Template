using BillCollector.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.Infrastructure.Repository
{
    public interface IDapperRepository
    {
        Task<IEnumerable<T>> GetManyAsync<T>(string query, object parameters = null);
        Task<T> GetAsync<T>(string query, object parameters);
        Task<int> InsertAsync(string query, object parameters);
        Task<int> UpdateAsync(string query, object parameters);
        Task<int> DeleteAsync(string query, object parameters);
    }
}
