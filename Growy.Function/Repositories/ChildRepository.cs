using System.Data;
using Dapper;
using Growy.Function.Entities;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Repositories.Interfaces;

namespace Growy.Function.Repositories;

public class ChildRepository(IConnectionFactory connectionFactory) : IChildRepository
{
    private const string ChildrenTable = "inventory.children";

    public async Task<Guid> GetHomeIdByChildId(Guid childId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"SELECT HomeId FROM {ChildrenTable} WHERE Id = @Id";
        return await con.QuerySingleAsync<Guid>(query, new { Id = childId });
    }

    public async Task<List<Child>> GetChildrenByHomeId(Guid homeId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"SELECT * FROM {ChildrenTable} WHERE HomeId = @HomeId";
        var childEntities = await con.QueryAsync<ChildEntity>(query, new { HomeId = homeId });
        return childEntities.Select(e => e.ToChild()).ToList();
    }

    public async Task<Guid> InsertChild(Guid homeId, ChildRequest request)
    {
        var childEntity = request.ToChildEntity();
        childEntity.HomeId = homeId;
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {ChildrenTable} (Name, HomeId, DOB, Gender, PointsEarned) VALUES (@Name, @HomeId, @DOB, @Gender, @PointsEarned) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, childEntity);
    }

    public async Task<Guid> EditPointsByChildId(Guid childId, int deltaPoints, IDbConnection con,
        IDbTransaction transaction)
    {
        var query =
            $"UPDATE {ChildrenTable} SET PointsEarned = PointsEarned + @PointsDelta WHERE Id = @Id RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, new { Id = childId, PointsDelta = deltaPoints },
            transaction: transaction);
    }

    public async Task<Guid> EditChild(Guid childId, ChildRequest child)
    {
        var childEntity = child.ToChildEntity();
        childEntity.Id = childId;
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 UPDATE {ChildrenTable} SET Name = @Name, 
                     DOB = @DOB,
                     Gender = @Gender
                 WHERE Id = @Id RETURNING Id;
             """;
        return await con.ExecuteScalarAsync<Guid>(query, childEntity);
    }

    public async Task DeleteChildByChildId(Guid childId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"DELETE FROM {ChildrenTable} WHERE Id = @Id RETURNING Id;";
        await con.ExecuteScalarAsync<Guid>(query, new { Id = childId });
    }
}