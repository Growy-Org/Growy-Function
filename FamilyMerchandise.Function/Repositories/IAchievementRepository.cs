using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public interface IAchievementRepository
{
    public Task<Guid> InsertAchievement(CreateAchievementRequest request);
}