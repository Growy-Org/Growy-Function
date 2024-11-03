using Dapper;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

public class PenaltyRepository(IConnectionFactory connectionFactory) : IPenaltyRepository
{
    private const string PenaltyTable = "inventory.penalties";

    public async Task<Guid> InsertPenalty(CreatePenaltyRequest request)
    {
        var penaltyEntity = request.ToPenaltyEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {PenaltyTable} (Name, HomeId, IconCode, PointsDeducted, Reason, ViolatorId, EnforcerId) VALUES (@Name, @HomeId, @IconCode, @PointsDeducted, @Reason, @ViolatorId, @EnforcerId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, penaltyEntity);
    }
}