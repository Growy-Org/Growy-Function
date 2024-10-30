using FamilyMerchandise.Function.Models;
namespace FamilyMerchandise.Function.Services;

public interface IHomeService
{
    // Children
    public Child AddChild();
    public Child UpdateChildInfo(Guid childId, Child child);
    public Child RemoveChild(Guid childId);
    
    // Parent
    public Parent AddParent();
    public Parent UpdateParentInfo(Guid parentId, Parent parent);
    public Parent RemoveParent(Guid parentId);
}