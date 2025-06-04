using System.Data.Common;
using Growy.Function.Exceptions;
using Growy.Function.Models.Dtos;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;
using Growy.Function.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class ParentService(
    IParentRepository parentRepository,
    ILogger<ParentService> logger)
    : IParentService
{
    # region Parents

    // Read
    public async Task<Guid> GetHomeIdByParentId(Guid parentId)
    {
        return await parentRepository.GetHomeIdByParentId(parentId);
    }

    // Create
    public async Task<Guid> AddParentToHome(Guid homeId, ParentRequest request)
    {
        logger.LogInformation($"Adding a new Parent to Home: {homeId}");
        var parentId = await parentRepository.InsertParent(homeId, request);
        logger.LogInformation($"Successfully added a parent : {parentId} to Home: {homeId}");
        return parentId;
    }

    // Update
    public async Task<Guid> EditParent(Guid parentId, ParentRequest request)
    {
        logger.LogInformation($"Editing Parent: {parentId}");
        var id = await parentRepository.EditParentByParentId(parentId, request);
        logger.LogInformation($"Successfully edit parent : {id}");
        return parentId;
    }

    // Delete
    public async Task DeleteParent(Guid parentId)
    {
        logger.LogInformation($"Deleting Parent: {parentId}");
        try
        {
            await parentRepository.DeleteParentByParentId(parentId);
        }
        catch (DbException e)
        {
            if (e.SqlState == "23503")
            {
                logger.LogWarning($"Failed to delete Parent: {parentId}", e.Message);
                throw new DeletionFailureException();
            }
        }

        logger.LogInformation($"Successfully delete Parent : {parentId}");
    }

    #endregion
}