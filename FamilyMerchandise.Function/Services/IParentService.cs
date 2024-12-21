using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Services;

public interface IParentService
{
    // Assignments
    public Task<List<Assignment>> GetAllAssignmentsByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<Guid> CreateAssignment(CreateAssignmentRequest request);
    public Task<Guid> EditAssignment(EditAssignmentRequest request);
    public Task<Guid> EditAssignmentCompleteStatus(Guid assignmentId, bool isCompleted);
    public Task DeleteAssignment(Guid assignmentId);
    public Task<Guid> CreateStepToAssignment(CreateStepRequest request);
    public Task<Guid> EditStep(EditStepRequest request);
    public Task<Guid> EditStepCompleteStatus(Guid stepId, bool isCompleted);
    public Task DeleteStep(Guid stepId);

    // Wishes
    public Task<List<Wish>> GetAllWishesByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<Guid> EditWish(EditWishRequest request);

    // Achievements
    public Task<List<Achievement>> GetAllAchievementByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<Guid> CreateAchievement(CreateAchievementRequest request);
    public Task<Guid> EditAchievement(EditAchievementRequest request);
    public Task<Guid> EditAchievementGrants(Guid achievementId, bool isAchievementGranted);
    public Task DeleteAchievement(Guid achievementId);

    // Penalties
    public Task<List<Penalty>> GetAllPenaltiesByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<Guid> CreatePenalty(CreatePenaltyRequest request);
    public Task<Guid> EditPenalty(EditPenaltyRequest request);
    public Task DeletePenalty(Guid penaltyId);
}