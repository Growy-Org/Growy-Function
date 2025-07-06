using Growy.Function.Entities.EntityResponse;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IAssignmentRepository
{

    public Task<int> GetAssignmentsCount(Guid homeId, Guid? parentId, Guid? childId, bool showOnlyIncomplete = false);
    public Task<Guid> GetHomeIdByAssignmentId(Guid assignmentId);
    public Task<Assignment> GetAssignmentById(Guid assignmentId);

    public Task<List<Assignment>> GetAllAssignments(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId, bool showOnlyIncomplete = false);

    public Task<Guid> InsertAssignment(Guid homeId, AssignmentRequest request);
    public Task<Guid> EditAssignmentByAssignmentId(Guid assignmentId, AssignmentRequest request);
    public Task<EditAssignmentEntityResponse> EditAssignmentCompleteStatus(Guid assignmentId, bool isCompleted);
    public Task DeleteAssignmentByAssignmentId(Guid assignmentId);
}