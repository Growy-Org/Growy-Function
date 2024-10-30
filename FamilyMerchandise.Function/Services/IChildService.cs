using FamilyMerchandise.Function.Models;

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
    public Wish CreateWish();
    public Wish EditWish(Guid wishId);
    
    // Achievements
    public List<Achievement> GetAchievementsByChildId(Guid childId);
    
    // Penalties
    public List<Penalty> GetPenaltiesByChildId(Guid childId);
}