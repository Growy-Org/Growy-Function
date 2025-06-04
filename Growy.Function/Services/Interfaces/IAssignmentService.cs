using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IAssignmentService
{
    // Read
    public Task<Assignment> GetAssignmentById(Guid assignmentId);
    public Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<List<Assignment>> GetAllAssignmentsByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<List<Assignment>> GetAllAssignmentsByChildId(Guid childId, int pageNumber, int pageSize);
    public Task<Guid> GetHomeIdByAssignmentId(Guid assignmentId);
    public Task<Guid> GetHomeIdByStepId(Guid stepId);

    // Create
    public Task<Guid> CreateAssignment(CreateAssignmentRequest request);
    public Task<Guid> CreateStepToAssignment(CreateStepRequest request);

    // Update
    public Task<Guid> EditAssignment(EditAssignmentRequest request);
    public Task<Guid> EditAssignmentCompleteStatus(Guid assignmentId, bool isCompleted);
    public Task<Guid> EditStep(EditStepRequest request);
    public Task<Guid> EditStepCompleteStatus(Guid stepId, bool isCompleted);

    // Delete
    public Task DeleteAssignment(Guid assignmentId);
    public Task DeleteStep(Guid stepId);
}