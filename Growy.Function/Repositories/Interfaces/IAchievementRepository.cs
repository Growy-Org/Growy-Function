using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IAchievementRepository
{
    public Task<Guid> GetHomeIdByAchievementId(Guid achievementId);
    public Task<List<Achievement>> GetAllAchievementsByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<Achievement> GetAchievementById(Guid achievementId);
    public Task<List<Achievement>> GetAllAchievementsByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId, int pageNumber, int pageSize);
    public Task<Guid> InsertAchievement(Guid homeId, AchievementRequest request);
    public Task<Guid> EditAchievementByAchievementId(Guid achievementId, AchievementRequest request);

    public Task<EditAchievementEntityResponse> EditAchievementGrantByAchievementId(Guid achievementId,
        bool isAchievementGranted);

    public Task DeleteAchievementByAchievementId(Guid achievementId);
}