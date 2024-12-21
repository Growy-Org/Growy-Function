using Dapper;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

public class PenaltyRepository(IConnectionFactory connectionFactory) : IPenaltyRepository
{
    private const string PenaltyTable = "inventory.penalties";
    private const string ChildrenTable = "inventory.children";
    private const string ParentTable = "inventory.parents";

    public async Task<List<Penalty>> GetAllPenaltiesByHomeId(Guid homeId, int pageNumber, int pageSize)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"""
                SELECT *
                FROM {PenaltyTable} penalty
                LEFT JOIN {ChildrenTable} c ON penalty.ViolatorId = c.Id
                LEFT JOIN {ParentTable} p ON penalty.EnforcerId = p.Id
                WHERE penalty.HomeId = @HomeId
                ORDER BY p.CreatedDateUtc ASC
                LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;

        var penaltyEntities =
            await con.QueryAsync(query, _mapEntitiesToPenaltyModel,
                new { HomeId = homeId });
        return penaltyEntities.ToList();
    }

    public async Task<List<Penalty>> GetAllPenaltiesByParentId(Guid parentId, int pageNumber, int pageSize)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"""
                SELECT *
                FROM {PenaltyTable} penalty
                LEFT JOIN {ChildrenTable} c ON penalty.ViolatorId = c.Id
                LEFT JOIN {ParentTable} p ON penalty.EnforcerId = p.Id
                WHERE penalty.EnforcerId = @EnforcerId
                ORDER BY p.CreatedDateUtc ASC
                LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;

        var penaltyEntities =
            await con.QueryAsync(query, _mapEntitiesToPenaltyModel,
                new { Enforcerid = parentId });
        return penaltyEntities.ToList();
    }

    public async Task<List<Penalty>> GetAllPenaltiesByChildId(Guid childId, int pageNumber, int pageSize)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"""
                SELECT *
                FROM {PenaltyTable} penalty
                LEFT JOIN {ChildrenTable} c ON penalty.ViolatorId = c.Id
                LEFT JOIN {ParentTable} p ON penalty.EnforcerId = p.Id
                WHERE penalty.ViolatorId = @ViolatorId
                ORDER BY p.CreatedDateUtc ASC
                LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;

        var penaltyEntities =
            await con.QueryAsync(query, _mapEntitiesToPenaltyModel,
                new { ViolatorId = childId });
        return penaltyEntities.ToList();
    }

    private readonly Func<PenaltyEntity, ChildEntity, ParentEntity, Penalty> _mapEntitiesToPenaltyModel =
        (pe, c, par) =>
        {
            var penalty = pe.ToPenalty();
            penalty.Violator = c.ToChild();
            penalty.Enforcer = par.ToParent();
            return penalty;
        };

    public async Task<CreatePenaltyEntityResponse> InsertPenalty(CreatePenaltyRequest request)
    {
        var penaltyEntity = request.ToPenaltyEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {PenaltyTable} (Name, HomeId, IconCode, PointsDeducted, Reason, ViolatorId, EnforcerId) VALUES (@Name, @HomeId, @IconCode, @PointsDeducted, @Reason, @ViolatorId, @EnforcerId) RETURNING PointsDeducted as Points, ViolatorId AS ChildId, Id";
        return await con.QuerySingleAsync<CreatePenaltyEntityResponse>(query, penaltyEntity);
    }

    public async Task<EditPenaltyEntityResponse> EditPenaltyByPenaltyId(EditPenaltyRequest request)
    {
        var penaltyEntity = request.ToPenaltyEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"""
                WITH Old AS (SELECT PointsDeducted, ViolatorId FROM {PenaltyTable} WHERE Id = @Id)
                UPDATE {PenaltyTable} 
                SET Name = @Name,
                 IconCode = @IconCode,
                 Reason = @Reason,
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
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"DELETE FROM {PenaltyTable} where id = @Id RETURNING PointsDeducted as Points, ViolatorId AS ChildId, Id;";
        return await con.QuerySingleAsync<DeletePenaltyEntityResponse>(query, new { Id = penaltyId });
    }
}