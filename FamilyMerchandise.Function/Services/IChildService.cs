using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Services;

public interface IChildService
{
    // Profile
    public Child GetProfileByChildId(Guid childId);
    
    // Assignments
    public List<Assignment> GetAllAssignmentsByChildId(Guid childId);
    public Assignment GetAssignment(Guid assignmentId);
    
    // Wishes
    public List<Wish> GetWishes();
    public Task<Guid> CreateWish(CreateWishRequest request);
    public Wish EditWish(Guid wishId);
    
    // Achievements
    public List<Achievement> GetAchievementsByChildId(Guid childId);
    
    // Penalties
    public List<Penalty> GetPenaltiesByChildId(Guid childId);
}