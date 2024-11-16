using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IAssignmentRepository
{
    public Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId);
    public Task<List<Assignment>> GetAllAssignmentsByParentId(Guid parentId);
    public Task<List<Assignment>> GetAllAssignmentsByChildId(Guid homeId);
    public Task<Guid> InsertAssignment(CreateAssignmentRequest request);
    public Task<Guid> EditAssignmentByAssignmentId(EditAssignmentRequest request);
    public Task<EditAssignmentEntityResponse> EditAssignmentCompleteStatus(Guid assignmentId, bool isCompleted);
}
