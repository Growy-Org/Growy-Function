using Dapper;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public class ParentRepository(IConnectionFactory connectionFactory) : IParentRepository
{
    public const string PARENTS_TABLE = "inventory.parents";

    public async Task<Guid> InsertParent(Guid homeId, Parent parent)
    {
        var parentEntity = parent.ToParentEntity(homeId);
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {PARENTS_TABLE} (Name, HomeId, IconCode, DOB, Role) VALUES (@Name, @HomeId, @IconCode, @DOB, @Role) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, parentEntity);
    }
}