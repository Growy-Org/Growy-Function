using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IAchievementService
{
    // Read 
    public Task<int> GetAchievementsCount(Guid homeId, Guid? parentId, Guid? childId, bool showOnlyNotAchieved = false);

    public Task<List<Achievement>> GetAllAchievements(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId, bool showOnlyNotAchieved = false);

    public Task<Guid> GetHomeIdByAchievementId(Guid achievementId);

    // Create
    public Task<Guid> CreateAchievement(Guid homeId, AchievementRequest request);

    // Update
    public Task<Guid> EditAchievement(Guid achievementId, AchievementRequest request);
    public Task<Guid> EditAchievementGrants(Guid achievementId, bool isAchievementGranted);

    // Delete
    public Task DeleteAchievement(Guid achievementId);
}