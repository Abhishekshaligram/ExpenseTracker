using Dapper;
using System.Data;

namespace PracticeCrud.Common.Config
{
    public interface IBaseRepository
    {
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<IEnumerable<T>>QueryAsync<T>(string sql,object? param=null,IDbTransaction?transaction=null,int? commandTimeout=null, CommandType? commandType = null);
        Task<int> ExecuteAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<SqlMapper.GridReader> QueryMultipleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        //Task<SqlMapper.GridReader> QueryMultipleAsyncForDiffConn<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task CloseConnAsync();
    }
}
