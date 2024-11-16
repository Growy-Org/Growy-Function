using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Services;

public interface IParentService
{
    // Assignments
    public Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId);
    public Task<Guid> CreateAssignment(CreateAssignmentRequest request);
    public Task<Guid> EditAssignment(EditAssignmentRequest request);
    public Task<Guid> EditAssignmentCompleteStatus(Guid assignmentId, bool isCompleted);
    public Task<Guid> CreateStepToAssignment(CreateStepRequest request);
    public Task<Guid> EditStep(EditStepRequest request);
    public Task<Guid> EditStepCompleteStatus(Guid stepId, bool isCompleted);

    // Wishes
    public Task<List<Wish>> GetAllWishesByHomeId(Guid homeId);
    public Task<Guid> EditWish(EditWishRequest request);

    // Achievements
    public Task<List<Achievement>> GetAllAchievementByHomeId(Guid homeId);
    public Task<Guid> CreateAchievement(CreateAchievementRequest request);
    public Task<Guid> EditAchievement(EditAchievementRequest request);
    public Task<Guid> EditAchievementGrants(Guid achievementId, bool isAchievementGranted);

    // Penalties
    public Task<List<Penalty>> GetAllPenaltiesByHomeId(Guid homeId);
    public Task<Guid> CreatePenalty(CreatePenaltyRequest request);
    public Task<Guid> EditPenalty(EditPenaltyRequest request);
}