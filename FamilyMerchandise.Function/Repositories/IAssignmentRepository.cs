using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public interface IAssignmentRepository
{
    public Task<Guid> InsertAssignment(CreateAssignmentRequest request);
}