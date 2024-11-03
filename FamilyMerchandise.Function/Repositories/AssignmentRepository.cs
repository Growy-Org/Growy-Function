using Dapper;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

public class AssignmentRepository(IConnectionFactory connectionFactory) : IAssignmentRepository
{
    private const string AssignmentsTable = "inventory.assignments";
    public const string ChildrenTable = "inventory.children";
    public const string ParentTable = "inventory.parents";

    public async Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            @$"
                SELECT *
                FROM {AssignmentsTable} a
                LEFT JOIN {ChildrenTable} c ON a.AssigneeId = c.Id
                LEFT JOIN {ParentTable} p ON a.AssignerId = p.Id
                WHERE a.HomeId = @HomeId
            ";
        var assignmentEntities = await con.QueryAsync(query, _mapEntitiesToAssignmentModel, new { HomeId = homeId });
        return assignmentEntities.ToList();
    }

    public async Task<List<Assignment>> GetAllAssignmentsByChildId(Guid childId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            @$"
                SELECT *
                FROM {AssignmentsTable} a
                LEFT JOIN {ChildrenTable} c ON a.AssigneeId = c.Id
                LEFT JOIN {ParentTable} p ON a.AssignerId = p.Id
                WHERE a.AssigneeId = @AssigneeId
            ";
        var assignmentEntities = await con.QueryAsync(query, _mapEntitiesToAssignmentModel, new { AssigneeId = childId });
        return assignmentEntities.ToList();
    }

    private readonly Func<AssignmentEntity, ChildEntity, ParentEntity, Assignment> _mapEntitiesToAssignmentModel =
        (a, c, p) =>
        {
            var assignment = a.ToAssignment();
            assignment.Assignee = c.ToChild();
            assignment.Assigner = p.ToParent();
            return assignment;
        };

    public async Task<Guid> InsertAssignment(CreateAssignmentRequest request)
    {
        var assignmentEntity = request.ToAssignmentEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {AssignmentsTable} (Name, HomeId, IconCode, Points, Description, RepeatAfter, DueDate, AssigneeId, AssignerId) VALUES (@Name, @HomeId, @IconCode, @Points, @Description, @RepeatAfter, @DueDate, @AssigneeId, @AssignerId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, assignmentEntity);
    }
}