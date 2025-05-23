using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IStepRepository
{
    public Task<List<Step>> GetAllStepsByAssignmentId(Guid assignmentId);
    public Task<Guid> InsertStep(CreateStepRequest request);
    public Task<Guid> EditStepByStepId(EditStepRequest request);
    public Task<Guid> EditStepCompleteStatusByStepId(Guid stepId, bool isCompleted);
    public Task DeleteStepByStepId(Guid stepId);
}