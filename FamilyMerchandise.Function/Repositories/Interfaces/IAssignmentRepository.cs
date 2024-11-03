using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IAssignmentRepository
{
    public Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId);
    public Task<List<Assignment>> GetAllAssignmentsByChildId(Guid homeId);
    public Task<Guid> InsertAssignment(CreateAssignmentRequest request);
}