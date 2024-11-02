using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Services;

public interface IParentService
{
    // Assignments
    public Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId);
    public Assignment GetAssignment(Guid assignmentId);
    public Task<Guid> CreateAssignment(CreateAssignmentRequest request);
    public void EditAssignment(Guid assignmentId, Assignment assignment);
    public void CompleteAssignment(Guid assignmentId);
    public Assignment CreateStepToAssignment(Guid assignmentId);
    public void EditStep(Guid stepId);

    // Wishes
    public List<Wish> GetWishesByHomeId(Guid homeId);
    public Wish EditWishCost(Guid wishId);

    // Achievements
    public List<Assignment> GetAllAchievementByHomeId(Guid homeId);
    public Task<Guid> CreateAchievement(CreateAchievementRequest request);
    public void EditAchievement(Guid achievementId);
    public void GrantAchievementBonus(Guid achievementId);

    // Penalties
    public List<Penalty> GetAllPenaltiesByHomeId(Guid homeId);
    public Task<Guid> CreatePenalty(CreatePenaltyRequest request);
}