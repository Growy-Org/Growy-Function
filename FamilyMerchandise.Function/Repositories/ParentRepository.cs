using Dapper;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;
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

    public async Task<Guid> EditParentByParentId(EditParentRequest request)
    {
        var parentEntity = request.ToParentEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"""
                 UPDATE {ParentsTable} SET Name = @Name, 
                     DOB = @DOB,
                     Role = @Role,
                     IconCode = @IconCode
                 WHERE Id = @Id RETURNING Id;
             """;
        return await con.ExecuteScalarAsync<Guid>(query, parentEntity);
    }
    
    public async Task DeleteParentByParentId(Guid parentId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"DELETE FROM {ParentsTable} WHERE Id = @Id RETURNING Id;";
        await con.ExecuteScalarAsync<Guid>(query, new {Id = parentId});
    }
}