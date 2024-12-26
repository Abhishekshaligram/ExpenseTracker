using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace PracticeCrud.Common.Config
{
    public class BaseRepository : IBaseRepository
    {
        public readonly IOptions<DataConfig> _connectionString;
        private SqlConnection Conn;

        public BaseRepository(IOptions<DataConfig> connectionString) {
            _connectionString = connectionString;
            string decryptedConn =_connectionString.Value.DefaultConnection ?? string.Empty;
            Conn = new SqlConnection(decryptedConn);

        }
      
        public async  Task<int> ExecuteAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {

            await Conn.OpenAsync();
            var R = await Conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
            await CloseConnAsync();
            return R;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            await Conn.OpenAsync();
            var R= await Conn.QueryAsync<T>(sql, param, commandType: CommandType.StoredProcedure);
            await Conn.CloseAsync();
            return R;
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            await Conn.OpenAsync();
            var R = await Conn.QueryFirstOrDefaultAsync<T>(sql, param, commandType: CommandType.StoredProcedure);
            await CloseConnAsync();
            return R;
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (Conn.State == ConnectionState.Closed)
                await Conn.OpenAsync();
            var R = await Conn.QueryMultipleAsync(sql, param, commandType: CommandType.StoredProcedure);
            return R;

        }

        public Task<SqlMapper.GridReader> QueryMultipleAsyncForDiffConn<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            throw new NotImplementedException();
        }
        public async Task CloseConnAsync()
        {
            if (Conn.State == ConnectionState.Open)
                await Conn.CloseAsync();
        }

    }
}
