using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IAssignmentRepository
{
    public Task<Assignment> GetAssignmentById(Guid assignmentId);
    public Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<List<Assignment>> GetAllAssignmentsByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<List<Assignment>> GetAllAssignmentsByChildId(Guid homeId, int pageNumber, int pageSize);
    public Task<Guid> InsertAssignment(CreateAssignmentRequest request);
    public Task<Guid> EditAssignmentByAssignmentId(EditAssignmentRequest request);
    public Task<EditAssignmentEntityResponse> EditAssignmentCompleteStatus(Guid assignmentId, bool isCompleted);
    public Task DeleteAssignmentByAssignmentId(Guid assignmentId);
}
