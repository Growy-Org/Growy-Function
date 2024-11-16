using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IAchievementRepository
{
    public Task<List<Achievement>> GetAllAchievementsByHomeId(Guid homeId);
    public Task<List<Achievement>> GetAllAchievementsByParentId(Guid parentId);
    public Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId);
    public Task<Guid> InsertAchievement(CreateAchievementRequest request);
    public Task<Guid> EditAchievementByAchievementId(EditAchievementRequest request);
    public Task<EditAchievementEntityResponse> EditAchievementGrantByAchievementId(Guid achievementId, bool isAchievementGranted);
}