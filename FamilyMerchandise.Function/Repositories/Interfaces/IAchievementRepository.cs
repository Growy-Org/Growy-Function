using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IAchievementRepository
{
    public Task<List<Achievement>> GetAllAchievementsByHomeId(Guid homeId);
    public Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId);
    public Task<Guid> InsertAchievement(CreateAchievementRequest request);
}