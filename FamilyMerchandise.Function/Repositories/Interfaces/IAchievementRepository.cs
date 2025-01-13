using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IAchievementRepository
{
    public Task<List<Achievement>> GetAllAchievementsByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<Achievement> GetAchievementById(Guid achievementId);
    public Task<List<Achievement>> GetAllAchievementsByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId, int pageNumber, int pageSize);
    public Task<Guid> InsertAchievement(CreateAchievementRequest request);
    public Task<Guid> EditAchievementByAchievementId(EditAchievementRequest request);
    public Task<EditAchievementEntityResponse> EditAchievementGrantByAchievementId(Guid achievementId,
        bool isAchievementGranted);
    public Task DeleteAchievementByAchievementId(Guid achievementId);
}