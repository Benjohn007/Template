using Dapper;
using BillCollector.Core.Enums;
using BillCollector.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.Infrastructure.Repository
{

    public class DapperRepository : IDapperRepository
    {
        private DapperContext _context;
        public DapperRepository(DapperContext context)
        {
            _context = context;
        }

        public IDbConnection GetContextConnection()
        {
            return _context.CreateConnection();
        }

        public async Task<IEnumerable<T>> GetManyAsync<T>(string query, object parameters = null)
        {
            using (var connection = GetContextConnection())
            {
                return await connection.QueryAsync<T>(query, parameters);
            }
        }

        public async Task<T> GetAsync<T>(string query, object parameters)
        {
            using (var connection = GetContextConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<T>(query, parameters);
            }
        }

        public async Task<int> InsertAsync(string query, object parameters)
        {
            using (var connection = GetContextConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(string query, object parameters)
        {
            using (var connection = GetContextConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string query, object parameters)
        {
            using (var connection = GetContextConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
