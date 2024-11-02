using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories;
using Microsoft.Extensions.Logging;

namespace FamilyMerchandise.Function.Services;

public class HomeService(
    IHomeRepository homeRepository,
    IChildRepository childRepository,
    IParentRepository parentRepository,
    ILogger<HomeService> logger)
    : IHomeService
{
    public async Task<Home> GetHomeInfoById(Guid homeId)
    {
        logger.LogInformation($"Getting Home by Id: {homeId}");
        var home = await homeRepository.GetHome(homeId);
        logger.LogInformation($"Getting Parents Info with HomeId: {homeId}");
        var parents = await parentRepository.GetParentsByHomeId(homeId);
        home.HydrateParents(parents);
        logger.LogInformation($"Getting Children Info with HomeId: {homeId}");
        var children = await childRepository.GetChildrenByHomeId(homeId);
        home.HydrateChildren(children);
        return home;
    }

    public async Task<Guid> CreateHome(Home home)
    {
        logger.LogInformation($"Adding a new Home: {home.Name}");
        var homeId = await homeRepository.InsertHome(home);
        logger.LogInformation($"Successfully added a home: {homeId}");
        return homeId;
    }

    public async Task<Guid> AddChildToHome(Guid homeId, Child child)
    {
        logger.LogInformation($"Adding a new Child to Home: {homeId}");
        var childId = await childRepository.InsertChild(homeId, child);
        logger.LogInformation($"Successfully added a child : {childId} to Home: {homeId}");
        return childId;
    }

    public async Task<Guid> AddParentToHome(Guid homeId, Parent parent)
    {
        logger.LogInformation($"Adding a new Parent to Home: {homeId}");
        var parentId = await parentRepository.InsertParent(homeId, parent);
        logger.LogInformation($"Successfully added a parent : {parentId} to Home: {homeId}");
        return parentId;
    }

    public void EditHome(Guid homeId)
    {
        throw new NotImplementedException();
    }

    public void RemoveHome(Guid homeId)
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