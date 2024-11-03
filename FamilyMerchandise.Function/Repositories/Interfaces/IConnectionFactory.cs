using System.Data;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IConnectionFactory
{
    public IDbConnection GetFamilyMerchandiseDBConnection();
}