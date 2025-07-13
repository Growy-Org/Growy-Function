using System.Data;
using Growy.Function.Entities.EntityResponse;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IAchievementRepository
{
    public Task<Guid> GetHomeIdByAchievementId(Guid achievementId);
    public Task<int> GetAchievementsCount(Guid homeId, Guid? parentId, Guid? childId, bool showOnlyNotAchieved = false);

    public Task<List<Achievement>> GetAllAchievements(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId, bool showOnlyNotAchieved = false);

    public Task<Guid> InsertAchievement(Guid homeId, AchievementRequest request);
    public Task<Guid> EditAchievementByAchievementId(Guid achievementId, AchievementRequest request);

    public Task<EditAchievementEntityResponse> EditAchievementGrantByAchievementId(Guid achievementId,
        bool isAchievementGranted, IDbConnection con, IDbTransaction transaction);

    public Task DeleteAchievementByAchievementId(Guid achievementId);
}