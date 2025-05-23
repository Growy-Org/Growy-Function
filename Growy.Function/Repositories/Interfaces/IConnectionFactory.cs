using System.Data;

namespace Growy.Function.Repositories.Interfaces;

public interface IConnectionFactory
{
    public IDbConnection GetFamilyMerchandiseDBConnection();
}