using System.Data;

namespace Growy.Function.Repositories.Interfaces;

public interface IConnectionFactory
{
    public Task<IDbConnection> GetDBConnection();
}