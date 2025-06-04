using System.Data.Common;
using Growy.Function.Exceptions;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Repositories.Interfaces;
using Growy.Function.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class HomeService(
    IHomeRepository homeRepository,
    IChildRepository childRepository,
    IParentRepository parentRepository,
    ILogger<HomeService> logger)
    : IHomeService
{
    #region Home

    // Read
    public async Task<List<Home>> GetHomesByAppUserId(Guid appUserId)
    {
        logger.LogInformation($"Getting home all homes by app user Id {appUserId}");
        var homes = await homeRepository.GetAllHomeByAppUserId(appUserId);
        logger.LogInformation($"Successfully get {homes.Count} by app user Id: {appUserId}");
        return homes;
    }

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

    // Create
    public async Task<Guid> CreateHome(CreateHomeRequest request)
    {
        logger.LogInformation($"Adding a new Home: {request.HomeName}");
        var homeId = await homeRepository.InsertHome(request);
        logger.LogInformation($"Successfully added a home: {homeId}");
        return homeId;
    }

    // Update
    public async Task<Guid> EditHome(EditHomeRequest request)
    {
        logger.LogInformation($"Editing Home: {request.HomeId}");
        var homeId = await homeRepository.EditHomeByHomeId(request);
        logger.LogInformation($"Successfully edit home : {homeId}");
        return homeId;
    }

    // Delete
    public async Task DeleteHome(Guid homeId)
    {
        logger.LogInformation($"Deleting Home: {homeId}");
        try
        {
            await homeRepository.DeleteHomeByHomeId(homeId);
        }
        catch (DbException e)
        {
            if (e.SqlState == "23503")
            {
                logger.LogWarning($"Failed to delete home: {homeId}", e.Message);
                throw new DeletionFailureException();
            }
        }

        logger.LogInformation($"Successfully delete Home : {homeId}");
    }

    #endregion
}