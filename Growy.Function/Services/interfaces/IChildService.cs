using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Services;

public interface IChildService
{
    // Children
    public Task<Guid> GetHomeIdByChildId(Guid id);
    public Task<Guid> AddChildToHome(Guid childId, Child child);
    public Task<Guid> EditChild(Child request);
    public Task DeleteChild(Guid childId);
    
    // Assignments
    public Task<List<Assignment>> GetAllAssignmentsByChildId(Guid childId, int pageNumber, int pageSize);

    // Wishes
    public Task<List<Wish>> GetAllWishesByChildId(Guid childId, int pageNumber, int pageSize);
    public Task<Guid> CreateWish(CreateWishRequest request);
    public Task<Guid> EditWish(EditWishRequest request);
    public Task<Guid> SetWishFulFilled(Guid wishId, bool isFulFilled);
    public Task DeleteWish(Guid wishId);

    // Achievements
    public Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId, int pageNumber, int pageSize);

    // Penalties
    public Task<List<Penalty>> GetAllPenaltiesByChildId(Guid childId, int pageNumber, int pageSize);
}