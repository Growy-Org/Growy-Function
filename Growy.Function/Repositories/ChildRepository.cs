using Dapper;
using Growy.Function.Entities;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Repositories.Interfaces;

namespace Growy.Function.Repositories;

public class ChildRepository(IConnectionFactory connectionFactory) : IChildRepository
{
    private const string ChildrenTable = "inventory.children";

    public async Task<Child> GetChildById(Guid childId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"SELECT * FROM {ChildrenTable} WHERE ChildId = @ChildId";
        var entity = await con.QuerySingleAsync<ChildEntity>(query, new { ChildId = childId });
        return entity.ToChild();
    }

    public async Task<List<Child>> GetChildrenByHomeId(Guid homeId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"SELECT * FROM {ChildrenTable} WHERE HomeId = @HomeId";
        var childEntities = await con.QueryAsync<ChildEntity>(query, new { HomeId = homeId });
        return childEntities.Select(e => e.ToChild()).ToList();
    }

    public async Task<Guid> InsertChild(Guid homeId, Child child)
    {
        var childEntity = child.ToChildEntity(homeId);
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {ChildrenTable} (Name, HomeId, IconCode, DOB, Gender, PointsEarned) VALUES (@Name, @HomeId, @IconCode, @DOB, @Gender, @PointsEarned) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, childEntity);
    }

    public async Task<Guid> EditPointsByChildId(Guid childId, int deltaPoints)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"UPDATE {ChildrenTable} SET PointsEarned = PointsEarned + @PointsDelta WHERE Id = @Id RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, new { Id = childId, PointsDelta = deltaPoints });
    }

    public async Task<Guid> EditChildByChildId(EditChildRequest request)
    {
        var childEntity = request.ToChildEntity();
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
                 UPDATE {ChildrenTable} SET Name = @Name, 
                     DOB = @DOB,
                     Gender = @Gender,
                     IconCode = @IconCode
                 WHERE Id = @Id RETURNING Id;
             """;
        return await con.ExecuteScalarAsync<Guid>(query, childEntity);
    }

    public async Task DeleteChildByChildId(Guid childId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"DELETE FROM {ChildrenTable} WHERE Id = @Id RETURNING Id;";
        await con.ExecuteScalarAsync<Guid>(query, new { Id = childId });
    }
}