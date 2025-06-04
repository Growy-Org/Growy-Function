using Growy.Function.Models.Dtos;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;
using Growy.Function.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class PenaltyService(
    IChildRepository childRepository,
    IPenaltyRepository penaltyRepository,
    ILogger<PenaltyService> logger)
    : IPenaltyService
{
    #region Penalties

    // Read
    public async Task<Guid> GetHomeIdByPenaltyId(Guid penaltyId)
    {
        return await penaltyRepository.GetHomeIdByPenaltyId(penaltyId);
    }

    public async Task<List<Penalty>> GetAllPenaltiesByParentId(Guid parentId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all penalties by Parent: {parentId}");
        var penalties = await penaltyRepository.GetAllPenaltiesByParentId(parentId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all penalties by Parent : {parentId}");
        return penalties;
    }

    public async Task<List<Penalty>> GetAllPenaltiesByChildId(Guid childId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all penalties by ChildId: {childId}");
        var penalties = await penaltyRepository.GetAllPenaltiesByChildId(childId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all penalties by ChildId : {childId}");
        return penalties;
    }

    public async Task<List<Penalty>> GetAllPenaltiesByHomeId(Guid homeId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all penalties by HomeId: {homeId}");
        var penalties = await penaltyRepository.GetAllPenaltiesByHomeId(homeId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all penalties by HomeId : {homeId}");
        return penalties;
    }

    public async Task<Penalty> GetPenaltyById(Guid penaltyId)
    {
        logger.LogInformation($"Getting penalty by Id: {penaltyId}");
        var penalty = await penaltyRepository.GetPenaltyById(penaltyId);

        logger.LogInformation(
            $"Successfully getting penalty by Id: {penalty.Id}");
        return penalty;
    }

    // Create
    public async Task<Guid> CreatePenalty(Guid homeId, PenaltyRequest request)
    {
        logger.LogInformation($"Adding a new Penalty to Home: {homeId}");
        var response = await penaltyRepository.InsertPenalty(homeId, request);
        var childId = await childRepository.EditPointsByChildId(response.ChildId, -response.Points);
        logger.LogInformation(
            $"Successfully reduce points from Child: {childId}");
        logger.LogInformation(
            $"Successfully added a Penalty : {response.Id}, Points deducted : {response.Points}, by Parent {request.ParentId} to Child {childId}");
        return response.Id;
    }

    // Update
    public async Task<Guid> EditPenalty(Guid penaltyId, PenaltyRequest request)
    {
        logger.LogInformation($"Editing penalty {penaltyId}");
        var response = await penaltyRepository.EditPenaltyByPenaltyId(penaltyId, request);
        // if two children are different, then just apply the change. Else, only make changes if points deducted has changed
        if (request.ChildId != response.OldChildId)
        {
            logger.LogInformation(
                $"Changes in two children's points detected, applying changes to child {request.ChildId} and reverting changes to {response.OldChildId}");

            // TODO: should make this a transactional request in the future.
            // deduct points from new child.
            var newChildId = await childRepository.EditPointsByChildId(request.ChildId, -request.PointsDeducted);
            logger.LogInformation(
                $"Successfully apply point change to new child: {newChildId} deducted points: {request.PointsDeducted}");

            // add back the points deducted
            var oldChildId = await childRepository.EditPointsByChildId(response.OldChildId, response.OldPointsDeducted);
            logger.LogInformation(
                $"Successfully apply point change to old child: {oldChildId} adding back points: {response.OldPointsDeducted}");
        }
        else if (request.PointsDeducted - response.OldPointsDeducted != 0)
        {
            logger.LogInformation(
                $"Changes in one child's points detected, applying points change to child {request.ChildId}");

            // both old and new child id are the same, Formula: Final Child Points = Original + (OldPointsDeducted - NewPointsDeducted)
            var childId = await childRepository.EditPointsByChildId(request.ChildId,
                response.OldPointsDeducted - request.PointsDeducted);
            logger.LogInformation(
                $"Successfully apply point change to child: {childId}");
        }

        return response.Id;
    }

    // Delete
    public async Task DeletePenalty(Guid penaltyId)
    {
        logger.LogInformation($"Deleting penalty {penaltyId}");
        var response = await penaltyRepository.DeletePenaltyByPenaltyId(penaltyId);
        logger.LogInformation(
            $"Successfully deleted Penalty {response.Id}");

        var childId = await childRepository.EditPointsByChildId(response.ChildId, response.Points);
        logger.LogInformation(
            $"Successfully add {response.Points} points back to Child: {childId}");
    }

    #endregion
}