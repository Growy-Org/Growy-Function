using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Services;

public interface IChildService
{
    // Assignments
    public Task<List<Assignment>> GetAllAssignmentsByChildId(Guid childId);
    
    // Wishes
    public Task<List<Wish>> GetAllWishesByChildId(Guid childId);
    public Task<Guid> CreateWish(CreateWishRequest request);
    public Task<Guid> EditWish(EditWishRequest request);
    public Task<Guid> SetWishFullFilled(Guid wishId, bool isFullFilled);
    public Task DeleteWish(Guid wishId);
    
    // Achievements
    public Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId);
    
    // Penalties
    public Task<List<Penalty>> GetAllPenaltiesByChildId(Guid childId);
}