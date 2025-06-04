using System.Data.Common;
using Growy.Function.Exceptions;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;
using Growy.Function.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class ChildService(
    IChildRepository childRepository,
    ILogger<ChildService> logger)
    : IChildService
{
    #region Children

    // Read
    public async Task<Guid> GetHomeIdByChildId(Guid childId)
    {
        return await childRepository.GetHomeIdByChildId(childId);
    }

    // Create
    public async Task<Guid> AddChildToHome(Guid homeId, Child child)
    {
        logger.LogInformation($"Adding a new Child to Home: {homeId}");
        var childId = await childRepository.InsertChild(homeId, child);
        logger.LogInformation($"Successfully added a child : {childId} to Home: {homeId}");
        return childId;
    }

    // Update
    public async Task<Guid> EditChild(Child child)
    {
        logger.LogInformation($"Editing Child: {child.Id}");
        var childId = await childRepository.EditChild(child);
        logger.LogInformation($"Successfully edit child : {childId}");
        return childId;
    }

    // Delete
    public async Task DeleteChild(Guid childId)
    {
        logger.LogInformation($"Deleting Child: {childId}");
        try
        {
            await childRepository.DeleteChildByChildId(childId);
        }
        catch (DbException e)
        {
            if (e.SqlState == "23503")
            {
                logger.LogWarning($"Failed to delete Child: {childId}", e.Message);
                throw new DeletionFailureException();
            }
        }

        logger.LogInformation($"Successfully delete Child : {childId}");
    }

    #endregion
}