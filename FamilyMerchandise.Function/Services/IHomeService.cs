using FamilyMerchandise.Function.Models;
namespace FamilyMerchandise.Function.Services;

public interface IHomeService
{
    // Home 
    public Task<Home> GetHomeInfoById(Guid homeId);
    public void EditHome(Guid homeId);
    public void RemoveHome(Guid homeId);
    
    // Children
    public Child AddChildToHome(Guid childId, Child child);
    public Child UpdateChildInfo(Guid childId, Child child);
    public Child RemoveChild(Guid childId);
    
    // Parent
    public Parent AddParentToHome(Guid parentId, Parent parent);
    public Parent UpdateParentInfo(Guid parentId, Parent parent);
    public Parent RemoveParent(Guid parentId);
}