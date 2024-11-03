using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Services;

public interface IParentService
{
    // Assignments
    public Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId);
    public Task<Guid> CreateAssignment(CreateAssignmentRequest request);
    public Task EditAssignment(Guid assignmentId, Assignment assignment);
    public Task CompleteAssignment(Guid assignmentId);
    public Task<Guid> CreateStepToAssignment(CreateStepRequest request);
    public Task EditStep(Guid stepId);

    // Wishes
    public Task<List<Wish>> GetAllWishesByHomeId(Guid homeId);
    public Task EditWishCost(Guid wishId);

    // Achievements
    public Task<List<Assignment>> GetAllAchievementByHomeId(Guid homeId);
    public Task<Guid> CreateAchievement(CreateAchievementRequest request);
    public Task EditAchievement(Guid achievementId);
    public Task GrantAchievementBonus(Guid achievementId);

    // Penalties
    public Task<List<Penalty>> GetAllPenaltiesByHomeId(Guid homeId);
    public Task<Guid> CreatePenalty(CreatePenaltyRequest request);
}