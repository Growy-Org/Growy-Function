using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IStepRepository
{
    public Task<List<Step>> GetAllStepsByAssignmentId(Guid assignmentId);
    public Task<Guid> InsertStep(CreateStepRequest request);
    public Task<Guid> EditStepByStepId(EditStepRequest request);
    public Task<Guid> EditStepCompleteStatusByStepId(Guid stepId, bool isCompleted);
}