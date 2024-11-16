using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Services;

public interface IHomeService
{
    // Home 
    public Task<Home> GetHomeInfoById(Guid homeId);
    public Task<Guid> CreateHome(Home home);
    public Task<Guid> EditHome(EditHomeRequest request);

    // Children
    public Task<Guid> AddChildToHome(Guid childId, Child child);
    public Task<Guid> EditChild(EditChildRequest request);

    // Parent
    public Task<Guid> AddParentToHome(Guid parentId, Parent parent);
    public Task<Guid> EditParent(EditParentRequest request);
}