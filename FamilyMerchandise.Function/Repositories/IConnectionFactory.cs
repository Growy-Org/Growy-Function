using System.Data;

namespace FamilyMerchandise.Function.Repositories;

public interface IConnectionFactory
{
    public IDbConnection GetFamilyMerchandiseDBConnection();
}