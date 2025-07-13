using System.Data;
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
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT HomeId FROM {AssignmentsTable} WHERE Id = @Id
             """;
        return await con.QuerySingleAsync<Guid>(query, new { Id = assignmentId });
    }

    public async Task<int> GetAssignmentsCount(Guid homeId, Guid? parentId, Guid? childId,
        bool showOnlyIncomplete = false)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT COUNT(*) FROM {AssignmentsTable} a WHERE a.HomeId = @HomeId {GetConditionQuery(parentId, childId, showOnlyIncomplete)};
             """;
        return await con.QuerySingleAsync<int>(query, new { HomeId = homeId, ChildId = childId, ParentId = parentId });
    }

    public async Task<List<Assignment>> GetAllAssignments(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId, bool showOnlyIncomplete = false)
    {
        using var con = await connectionFactory.GetDBConnection();

        var query =
            $"""
                 SELECT *
                 FROM {AssignmentsTable} a
                 LEFT JOIN {ChildrenTable} c ON a.AssigneeId = c.Id
                 LEFT JOIN {ParentTable} p ON a.AssignerId = p.Id
                 WHERE a.HomeId = @HomeId
                 {GetConditionQuery(parentId, childId, showOnlyIncomplete)}
                 ORDER BY a.CreatedDateUtc ASC
                 LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize} 
             """;
        var assignmentEntities = await con.QueryAsync(query, _mapEntitiesToAssignmentModel,
            new { HomeId = homeId, ChildId = childId, ParentId = parentId });
        return assignmentEntities.ToList();
    }

    public async Task<Guid> InsertAssignment(Guid homeId, AssignmentRequest request)
    {
        var assignmentEntity = request.ToAssignmentEntity();
        assignmentEntity.HomeId = homeId;
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {AssignmentsTable} (Name, HomeId, Points, Description, DueDateUtc, AssigneeId, AssignerId) VALUES (@Name, @HomeId, @Points, @Description, @DueDateUtc, @AssigneeId, @AssignerId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, assignmentEntity);
    }

    public async Task<Guid> EditAssignmentByAssignmentId(Guid assignmentId, AssignmentRequest request)
    {
        var assignmentEntity = request.ToAssignmentEntity();
        assignmentEntity.Id = assignmentId;
        using var con = await connectionFactory.GetDBConnection();
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

    public async Task<EditAssignmentEntityResponse> EditAssignmentCompleteStatus(Guid assignmentId, bool isCompleted,
        IDbConnection con,
        IDbTransaction transaction)
    {
        var query =
            $"UPDATE {AssignmentsTable} SET CompletedDateUtc = @CompletedDateUtc WHERE Id = @Id RETURNING Id, AssigneeId AS ChildId, Points;";
        return await con.QuerySingleAsync<EditAssignmentEntityResponse>(query,
            new { Id = assignmentId, CompletedDateUtc = isCompleted ? DateTime.UtcNow : (DateTime?)null },
            transaction: transaction);
    }

    public async Task DeleteAssignmentByAssignmentId(Guid assignmentId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query = $"DELETE FROM {AssignmentsTable} WHERE Id = @Id;";
        await con.ExecuteScalarAsync<Guid>(query, new { Id = assignmentId });
    }

    private string GetConditionQuery(Guid? parentId, Guid? childId, bool showOnlyIncomplete)
    {
        var extraQuery = "";
        if (parentId != null) extraQuery += "AND a.AssignerId = @ParentId ";
        if (childId != null) extraQuery += "AND a.AssigneeId = @ChildId ";
        if (showOnlyIncomplete) extraQuery += "AND a.CompletedDateUtc IS NULL";
        return extraQuery;
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