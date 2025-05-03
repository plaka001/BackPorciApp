using System.Data;

namespace Aplicacion.Abstractions.Data;

public interface IDapperWrapper
{
    Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param);
}
