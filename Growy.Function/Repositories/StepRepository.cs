using Dapper;
using Growy.Function.Models.Dtos;
using Growy.Function.Entities;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;

namespace Growy.Function.Repositories;

public class StepRepository(IConnectionFactory connectionFactory) : IStepRepository
{
    private const string StepsTable = "inventory.steps";

    public async Task<List<Step>> GetAllStepsByAssignmentId(Guid assignmentId)
    {
        using var con = connectionFactory.GetDBConnection();
        // Sort steps in ascending order
        var query =
            $"SELECT * FROM {StepsTable} WHERE AssignmentId = @AssignmentId ORDER BY StepOrder";
        var stepEntities = await con.QueryAsync<StepEntity>(query, new { AssignmentId = assignmentId });
        return stepEntities.Select(e => e.ToStep()).ToList();
    }

    public async Task<Guid> InsertStep(CreateStepRequest request)
    {
        var stepEntity = request.ToStepEntity();
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {StepsTable} (Description, AssignmentId, StepOrder) VALUES (@Description, @AssignmentId, @StepOrder) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, stepEntity);
    }

    public async Task<Guid> EditStepByStepId(EditStepRequest request)
    {
        var stepEntity = request.ToStepEntity();
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"UPDATE {StepsTable} SET Description = @Description WHERE Id = @Id RETURNING Id;";
        return await con.ExecuteScalarAsync<Guid>(query, stepEntity);
    }

    public async Task<Guid> EditStepCompleteStatusByStepId(Guid stepId,
        bool isCompleted)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"UPDATE {StepsTable} SET CompletedDateUtc = @CompletedDateUtc WHERE Id = @Id RETURNING Id;";
        return await con.ExecuteScalarAsync<Guid>(query,
            new { Id = stepId, CompletedDateUtc = isCompleted ? DateTime.UtcNow : (DateTime?)null });
    }

    public async Task DeleteStepByStepId(Guid stepId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query = $"DELETE FROM {StepsTable} where id = @Id;";
        await con.ExecuteScalarAsync<Guid>(query, new { Id = stepId });
    }
}