using Dapper;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public class AssignmentRepository(IConnectionFactory connectionFactory) : IAssignmentRepository
{
    public const string ASSIGNMENTS_TABLE = "inventory.assignments";

    public async Task<Guid> InsertAssignment(CreateAssignmentRequest request)
    {
        var assignmentEntity = request.ToAssignmentEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {ASSIGNMENTS_TABLE} (Name, HomeId, IconCode, Points, Description, RepeatAfter, DueDate, AssigneeId, AssignerId) VALUES (@Name, @HomeId, @IconCode, @Points, @Description, @RepeatAfter, @DueDate, @AssigneeId, @AssignerId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, assignmentEntity);
    }
}