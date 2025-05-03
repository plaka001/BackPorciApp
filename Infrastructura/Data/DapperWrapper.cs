using Aplicacion.Abstractions.Data;
using Dapper;
using System.Data;

namespace Infrastructura.Data;

internal class DapperWrapper : IDapperWrapper
{
    public Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param)
    {
        return connection.QueryAsync<T>(sql, param);
    }
}
