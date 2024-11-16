using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Repositories.Interfaces;
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
        logger.LogInformation($"Successfully getting information for Home: {homeId}");
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

    public async Task<Guid> EditHome(EditHomeRequest request)
    {
        logger.LogInformation($"Editing Home: {request.HomeId}");
        var homeId = await homeRepository.EditHomeByHomeId(request);
        logger.LogInformation($"Successfully edit home : {homeId}");
        return homeId;
    }

    public async Task<Guid> EditParent(EditParentRequest request)
    {
        logger.LogInformation($"Editing Parent: {request.ParentId}");
        var parentId = await parentRepository.EditParentByParentId(request);
        logger.LogInformation($"Successfully edit parent : {parentId}");
        return parentId;
    }

    public async Task<Guid> EditChild(EditChildRequest request)
    {
        logger.LogInformation($"Editing Child: {request.ChildId}");
        var childId = await childRepository.EditChildByChildId(request);
        logger.LogInformation($"Successfully edit child : {childId}");
        return childId;
    }
}