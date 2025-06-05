using Dapper;
using Growy.Function.Entities;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Repositories.Interfaces;

namespace Growy.Function.Repositories;

public class ParentRepository(IConnectionFactory connectionFactory) : IParentRepository
{
    private const string ParentsTable = "inventory.parents";

    public async Task<Guid> GetHomeIdByParentId(Guid parentId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"SELECT HomeId FROM {ParentsTable} WHERE Id = @Id";
        return await con.QuerySingleAsync<Guid>(query, new { Id = parentId });
    }

    public async Task<List<Parent>> GetParentsByHomeId(Guid homeId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"SELECT * FROM {ParentsTable} WHERE HomeId = @HomeId";
        var parentEntities = await con.QueryAsync<ParentEntity>(query, new { HomeId = homeId });
        return parentEntities.Select(e => e.ToParent()).ToList();
    }

    public async Task<Guid> InsertParent(Guid homeId, ParentRequest request)
    {
        var parentEntity = request.ToParentEntity();
        parentEntity.HomeId = homeId;
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {ParentsTable} (Name, HomeId, DOB, Role) VALUES (@Name, @HomeId, @DOB, @Role) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, parentEntity);
    }

    public async Task<Guid> EditParentByParentId(Guid parentId, ParentRequest request)
    {
        var parentEntity = request.ToParentEntity();
        parentEntity.Id = parentId;
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
                 UPDATE {ParentsTable} SET Name = @Name, 
                     DOB = @DOB,
                     Role = @Role,
                 WHERE Id = @Id RETURNING Id;
             """;
        return await con.ExecuteScalarAsync<Guid>(query, parentEntity);
    }

    public async Task DeleteParentByParentId(Guid parentId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"DELETE FROM {ParentsTable} WHERE Id = @Id RETURNING Id;";
        await con.ExecuteScalarAsync<Guid>(query, new { Id = parentId });
    }
}