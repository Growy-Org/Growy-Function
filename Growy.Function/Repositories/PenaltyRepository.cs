using Dapper;
using Growy.Function.Models.Dtos;
using Growy.Function.Entities;
using Growy.Function.Entities.EntityResponse;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;

namespace Growy.Function.Repositories;

public class PenaltyRepository(IConnectionFactory connectionFactory) : IPenaltyRepository
{
    private const string PenaltyTable = "inventory.penalties";
    private const string ChildrenTable = "inventory.children";
    private const string ParentTable = "inventory.parents";

    public async Task<Guid> GetHomeIdByPenaltyId(Guid penaltyId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT HomeId FROM {PenaltyTable} WHERE Id = @Id
             """;
        return await con.QuerySingleAsync<Guid>(query, new { Id = penaltyId });
    }

    public async Task<int> GetPenaltiesCount(Guid homeId, Guid? parentId, Guid? childId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT COUNT(*) FROM {PenaltyTable} penalty WHERE penalty.HomeId = @HomeId {GetConditionQuery(parentId, childId)};
             """;
        return await con.QuerySingleAsync<int>(query, new { HomeId = homeId, ChildId = childId, ParentId = parentId });
    }

    public async Task<List<Penalty>> GetAllPenalties(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                SELECT *
                FROM {PenaltyTable} penalty
                LEFT JOIN {ChildrenTable} c ON penalty.ViolatorId = c.Id
                LEFT JOIN {ParentTable} p ON penalty.EnforcerId = p.Id
                WHERE penalty.HomeId = @HomeId
                {GetConditionQuery(parentId, childId)}
                ORDER BY p.CreatedDateUtc ASC
                LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;

        var penaltyEntities =
            await con.QueryAsync(query, _mapEntitiesToPenaltyModel,
                new { HomeId = homeId });
        return penaltyEntities.ToList();
    }


    public async Task<CreatePenaltyEntityResponse> InsertPenalty(Guid homeId, PenaltyRequest request)
    {
        var penaltyEntity = request.ToPenaltyEntity();
        penaltyEntity.HomeId = homeId;
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {PenaltyTable} (Name, HomeId, PointsDeducted, Description, ViolatorId, EnforcerId) VALUES (@Name, @HomeId, @PointsDeducted, @Description, @ViolatorId, @EnforcerId) RETURNING PointsDeducted as Points, ViolatorId AS ChildId, Id";
        return await con.QuerySingleAsync<CreatePenaltyEntityResponse>(query, penaltyEntity);
    }

    public async Task<EditPenaltyEntityResponse> EditPenaltyByPenaltyId(Guid penaltyId, PenaltyRequest request)
    {
        var penaltyEntity = request.ToPenaltyEntity();
        penaltyEntity.Id = penaltyId;
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                WITH Old AS (SELECT PointsDeducted, ViolatorId FROM {PenaltyTable} WHERE Id = @Id)
                UPDATE {PenaltyTable} 
                SET Name = @Name,
                 Description = @Description,
                 PointsDeducted = @PointsDeducted,
                 EnforcerId = @EnforcerId,
                 ViolatorId = @ViolatorId
                WHERE Id = @Id
                RETURNING Id, 
                (SELECT PointsDeducted FROM Old) AS OldPointsDeducted,
                (SELECT ViolatorId FROM Old) AS OldChildId;
             """;
        return await con.QuerySingleAsync<EditPenaltyEntityResponse>(query, penaltyEntity);
    }

    public async Task<DeletePenaltyEntityResponse> DeletePenaltyByPenaltyId(Guid penaltyId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"DELETE FROM {PenaltyTable} where id = @Id RETURNING PointsDeducted as Points, ViolatorId AS ChildId, Id;";
        return await con.QuerySingleAsync<DeletePenaltyEntityResponse>(query, new { Id = penaltyId });
    }

    private string GetConditionQuery(Guid? parentId, Guid? childId)
    {
        var extraQuery = "";
        if (parentId != null) extraQuery += "AND penalty.EnforcerId = @ParentId ";
        if (childId != null) extraQuery += "AND penalty.ViolatorId = @ChildId ";
        return extraQuery;
    }

    private readonly Func<PenaltyEntity, ChildEntity, ParentEntity, Penalty> _mapEntitiesToPenaltyModel =
        (pe, c, par) =>
        {
            var penalty = pe.ToPenalty();
            penalty.Violator = c.ToChild();
            penalty.Enforcer = par.ToParent();
            return penalty;
        };
}