using Dapper;
using Growy.Function.Models.Dtos;
using Growy.Function.Entities;
using Growy.Function.Entities.EntityResponse;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;

namespace Growy.Function.Repositories;

public class AssignmentRepository(IConnectionFactory connectionFactory) : IAssignmentRepository
{
    private const string AssignmentsTable = "inventory.assignments";
    private const string ChildrenTable = "inventory.children";
    private const string ParentTable = "inventory.parents";

    public async Task<Guid> GetHomeIdByAssignmentId(Guid assignmentId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT HomeId FROM {AssignmentsTable} WHERE Id = @Id
             """;
        return await con.QuerySingleAsync<Guid>(query, new { Id = assignmentId });
    }

    public async Task<Assignment> GetAssignmentById(Guid assignmentId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {AssignmentsTable} a
                 LEFT JOIN {ChildrenTable} c ON a.AssigneeId = c.Id
                 LEFT JOIN {ParentTable} p ON a.AssignerId = p.Id
                 WHERE a.Id = @Id
             """;
        var assignments = await con.QueryAsync(query, _mapEntitiesToAssignmentModel, new { Id = assignmentId });
        return assignments.Single();
    }

    public async Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId, int pageNumber, int pageSize)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {AssignmentsTable} a
                 LEFT JOIN {ChildrenTable} c ON a.AssigneeId = c.Id
                 LEFT JOIN {ParentTable} p ON a.AssignerId = p.Id
                 WHERE a.HomeId = @HomeId
                 ORDER BY a.CreatedDateUtc ASC
                 LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize} 
             """;
        var assignmentEntities = await con.QueryAsync(query, _mapEntitiesToAssignmentModel,
            new { HomeId = homeId });
        return assignmentEntities.ToList();
    }

    public async Task<List<Assignment>> GetAllAssignmentsByParentId(Guid parentId, int pageNumber, int pageSize)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {AssignmentsTable} a
                 LEFT JOIN {ChildrenTable} c ON a.AssigneeId = c.Id
                 LEFT JOIN {ParentTable} p ON a.AssignerId = p.Id
                 WHERE a.AssignerId = @AssignerId
                 ORDER BY a.CreatedDateUtc ASC
                 LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;
        var assignmentEntities =
            await con.QueryAsync(query, _mapEntitiesToAssignmentModel, new { AssignerId = parentId });
        return assignmentEntities.ToList();
    }

    public async Task<List<Assignment>> GetAllAssignmentsByChildId(Guid childId, int pageNumber, int pageSize)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {AssignmentsTable} a
                 LEFT JOIN {ChildrenTable} c ON a.AssigneeId = c.Id
                 LEFT JOIN {ParentTable} p ON a.AssignerId = p.Id
                 WHERE a.AssigneeId = @AssigneeId
                 ORDER BY a.CreatedDateUtc ASC
                 LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;
        var assignmentEntities =
            await con.QueryAsync(query, _mapEntitiesToAssignmentModel, new { AssigneeId = childId });
        return assignmentEntities.ToList();
    }

    public async Task<Guid> InsertAssignment(Guid homeId, AssignmentRequest request)
    {
        var assignmentEntity = request.ToAssignmentEntity();
        assignmentEntity.HomeId = homeId;
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {AssignmentsTable} (Name, HomeId, Points, Description, DueDateUtc, AssigneeId, AssignerId) VALUES (@Name, @HomeId, @Points, @Description, @DueDateUtc, @AssigneeId, @AssignerId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, assignmentEntity);
    }

    public async Task<Guid> EditAssignmentByAssignmentId(Guid assignmentId, AssignmentRequest request)
    {
        var assignmentEntity = request.ToAssignmentEntity();
        assignmentEntity.Id = assignmentId;
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"""
                 UPDATE {AssignmentsTable} SET Name = @Name,
                     Description = @Description,
                     Points = @Points,
                     DueDateUtc = @DueDateUtc,
                     AssigneeId = @AssigneeId,
                     AssignerId = @AssignerId
                 WHERE Id = @Id RETURNING Id;
             """;
        return await con.ExecuteScalarAsync<Guid>(query, assignmentEntity);
    }

    public async Task<EditAssignmentEntityResponse> EditAssignmentCompleteStatus(Guid assignmentId, bool isCompleted)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"UPDATE {AssignmentsTable} SET CompletedDateUtc = @CompletedDateUtc WHERE Id = @Id RETURNING Id, AssigneeId AS ChildId, Points;";
        return await con.QuerySingleAsync<EditAssignmentEntityResponse>(query,
            new { Id = assignmentId, CompletedDateUtc = isCompleted ? DateTime.UtcNow : (DateTime?)null });
    }

    public async Task DeleteAssignmentByAssignmentId(Guid assignmentId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query = $"DELETE FROM {AssignmentsTable} WHERE Id = @Id;";
        await con.ExecuteScalarAsync<Guid>(query, new { Id = assignmentId });
    }

    private readonly Func<AssignmentEntity, ChildEntity, ParentEntity, Assignment> _mapEntitiesToAssignmentModel =
        (a, c, p) =>
        {
            var assignment = a.ToAssignment();
            assignment.Assignee = c.ToChild();
            assignment.Assigner = p.ToParent();
            return assignment;
        };
}