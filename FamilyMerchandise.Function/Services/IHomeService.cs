using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Services;

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
    public Task<List<Wish>> GetAllWishesByHomeId(Guid homeId);
    public Task<List<Achievement>> GetAllAchievementByHomeId(Guid homeId);
    public Task<List<Penalty>> GetAllPenaltiesByHomeId(Guid homeId);
    public Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId);
}