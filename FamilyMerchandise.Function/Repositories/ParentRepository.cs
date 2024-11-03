using Dapper;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

public class ParentRepository(IConnectionFactory connectionFactory) : IParentRepository
{
    private const string ParentsTable = "inventory.parents";

    public async Task<List<Parent>> GetParentsByHomeId(Guid homeId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"SELECT * FROM {ParentsTable} WHERE HomeId = @HomeId";
        var parentEntities = await con.QueryAsync<ParentEntity>(query, new { HomeId = homeId });
        return parentEntities.Select(e => e.ToParent()).ToList();
    }

    public async Task<Guid> InsertParent(Guid homeId, Parent parent)
    {
        var parentEntity = parent.ToParentEntity(homeId);
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {ParentsTable} (Name, HomeId, IconCode, DOB, Role) VALUES (@Name, @HomeId, @IconCode, @DOB, @Role) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, parentEntity);
    }
}