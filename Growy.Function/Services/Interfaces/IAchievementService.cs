using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IAchievementService
{
    // Read 
    public Task<List<Achievement>> GetAllAchievementsByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<List<Achievement>> GetAllAchievementsByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId, int pageNumber, int pageSize);
    public Task<Achievement> GetAchievementById(Guid achievementId);
    public Task<Guid> GetHomeIdByAchievementId(Guid achievementId);

    // Create
    public Task<Guid> CreateAchievement(Guid homeId, AchievementRequest request);

    // Update
    public Task<Guid> EditAchievement(Guid achievementId, AchievementRequest request);
    public Task<Guid> EditAchievementGrants(Guid achievementId, bool isAchievementGranted);

    // Delete
    public Task DeleteAchievement(Guid achievementId);
}