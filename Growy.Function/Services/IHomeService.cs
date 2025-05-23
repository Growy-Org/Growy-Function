using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Services;

public interface IHomeService
{
    // Home 
    public Task<Home> GetHomeInfoById(Guid homeId);
    public Task<Guid> CreateHome(CreateHomeRequest home);
    public Task<Guid> EditHome(EditHomeRequest request);
    public Task DeleteHome(Guid homeId);

    // Children
    public Task<Guid> AddChildToHome(Guid childId, Child child);
    public Task<Guid> EditChild(EditChildRequest request);
    public Task DeleteChild(Guid childId);

    // Parent
    public Task<Guid> AddParentToHome(Guid parentId, Parent parent);
    public Task<Guid> EditParent(EditParentRequest request);
    public Task DeleteParent(Guid parentId);

    // Others
    public Task<List<Wish>> GetAllWishesByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<Wish> GetWishById(Guid wishId);
    public Task<List<Achievement>> GetAllAchievementByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<Achievement> GetAchievementById(Guid achievementId);
    public Task<List<Penalty>> GetAllPenaltiesByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<Penalty> GetPenaltyById(Guid penaltyId);
    public Task<Assignment> GetAssignmentById(Guid assignmentId);
    public Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId, int pageNumber, int pageSize);
}