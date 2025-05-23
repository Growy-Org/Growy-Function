using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services;

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
    public Task<Guid> CreateWish(CreateWishRequest request);
    public Task<Guid> EditWish(EditWishRequest request);
    public Task<Guid> SetWishFulFilled(Guid wishId, bool isFulFilled);
    public Task DeleteWish(Guid wishId);

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

    // Assessments
    public Task<Guid> SubmitDevelopmentQuotientReport(SubmitDevelopmentReportRequest request);
}