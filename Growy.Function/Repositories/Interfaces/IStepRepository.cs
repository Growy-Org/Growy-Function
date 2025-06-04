using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IStepRepository
{
    public Task<Guid> GetAssignmentIdByStepId(Guid stepId);
    public Task<List<Step>> GetAllStepsByAssignmentId(Guid assignmentId);
    public Task<Guid> InsertStep(Guid assignmentId, StepRequest request);
    public Task<Guid> EditStepByStepId(Guid stepId, StepRequest request);
    public Task<Guid> EditStepCompleteStatusByStepId(Guid stepId, bool isCompleted);
    public Task DeleteStepByStepId(Guid stepId);
}