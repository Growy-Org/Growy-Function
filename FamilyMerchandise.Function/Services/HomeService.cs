using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories;

namespace FamilyMerchandise.Function.Services;

public class HomeService(IHomeRepository repository) : IHomeService
{
    public Child AddChild()
    {
        throw new NotImplementedException();
    }

    public async Task<Home> GetHomeInfoById(Guid homeId)
    {
        return await repository.GetHome(homeId);
    }

    public void EditHome(Guid homeId)
    {
        throw new NotImplementedException();
    }

    public void RemoveHome(Guid homeId)
    {
        throw new NotImplementedException();
    }

    public Child AddChildToHome(Guid childId, Child child)
    {
        throw new NotImplementedException();
    }

    public Child UpdateChildInfo(Guid childId, Child child)
    {
        throw new NotImplementedException();
    }

    public Child RemoveChild(Guid childId)
    {
        throw new NotImplementedException();
    }

    public Parent AddParentToHome(Guid parentId, Parent parent)
    {
        throw new NotImplementedException();
    }

    public Child UpdateChildInfo()
    {
        throw new NotImplementedException();
    }

    public Child RemoveAChild()
    {
        throw new NotImplementedException();
    }

    public Parent AddParent()
    {
        throw new NotImplementedException();
    }

    public Parent UpdateParentInfo(Guid parentId, Parent parent)
    {
        throw new NotImplementedException();
    }

    public Parent RemoveParent(Guid parentId)
    {
        throw new NotImplementedException();
    }

    public Child UpdateParentInfo()
    {
        throw new NotImplementedException();
    }
}