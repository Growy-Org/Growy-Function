using Dapper;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

public class StepRepository(IConnectionFactory connectionFactory) : IStepRepository
{
    private const string StepsTable = "inventory.steps";

    public async Task<List<Step>> GetAllStepsByAssignmentId(Guid assignmentId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"SELECT * FROM {StepsTable} WHERE AssignmentId = @AssignmentId";
        var stepEntities = await con.QueryAsync<StepEntity>(query, new { AssignmentId = assignmentId });
        return stepEntities.Select(e => e.ToStep()).ToList();
    }

    public async Task<Guid> InsertStep(CreateStepRequest request)
    {
        var stepEntity = request.ToStepEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {StepsTable} (Description, AssignmentId, StepOrder) VALUES (@Description, @AssignmentId, @StepOrder) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, stepEntity);
    }
}