using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FamilyMerchandise.Function.Services;

public class ChildService(
    IChildRepository childRepository,
    IAssignmentRepository assignmentRepository,
    IStepRepository stepRepository,
    IAchievementRepository achievementRepository,
    IPenaltyRepository penaltyRepository,
    IWishRepository wishRepository,
    ILogger<ParentService> logger)
    : IChildService
{
    public Task<Child> GetProfileByChildId(Guid childId)
    {
        throw new NotImplementedException();
    }

    #region Assignments

    public async Task<List<Assignment>> GetAllAssignmentsByChildId(Guid childId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all assignments by ChildId: {childId}");
        var assignments = await assignmentRepository.GetAllAssignmentsByChildId(childId, pageNumber, pageSize);

        foreach (var assignment in assignments)
        {
            logger.LogInformation($"Getting Children Info with ChildId: {childId}");
            var steps = await stepRepository.GetAllStepsByAssignmentId(assignment.Id);
            assignment.SetSteps(steps);
        }

        logger.LogInformation(
            $"Successfully getting all assignments by ChildId : {childId}");
        return assignments;
    }

    #endregion

    #region Achievements

    public async Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all achievements by ChildId: {childId}");
        var achievements = await achievementRepository.GetAllAchievementsByChildId(childId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all achievements by ChildId : {childId}");
        return achievements;
    }

    #endregion

    #region Wishes

    public async Task<List<Wish>> GetAllWishesByChildId(Guid childId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all wishes by ChildId: {childId}");
        var wishes = await wishRepository.GetAllWishesByChildId(childId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all wishes by ChildId : {childId}");
        return wishes;
    }

    public async Task<Guid> CreateWish(CreateWishRequest request)
    {
        logger.LogInformation($"Adding a new wish to Home: {request.HomeId}  by Child");
        var wishId = await wishRepository.InsertWish(request);
        logger.LogInformation(
            $"Successfully added a wish : {wishId}, by Child {request.ChildId} to Parent {request.ParentId}");
        return wishId;
    }

    public async Task<Guid> EditWish(EditWishRequest request)
    {
        logger.LogInformation($"Editing wish {request.WishId} for Child");
        var id = await wishRepository.EditWishByWishId(request);
        logger.LogInformation(
            $"Successfully wish edited {request.WishId} by Child");
        return id;
    }

    public async Task<Guid> SetWishFulFilled(Guid wishId, bool isFulFilled)
    {
        logger.LogInformation($"{(isFulFilled ? "Full Filling" : "Un Full Filling")} wish by Child");
        var response =
            await wishRepository.EditWishFulFillStatusByWishId(wishId, isFulFilled);
        logger.LogInformation(
            $"Successfully edit full fill status with id: {response.Id} by Child");

        var childId = await childRepository.EditPointsByChildId(response.ChildId,
            isFulFilled ? -response.Points : response.Points);

        logger.LogInformation(
            $"Successfully {(isFulFilled ? "reducing" : "adding")} {response.Points} Points {(isFulFilled ? "from" : "back")} child profile with id: {childId} by Child");
        return response.Id;
    }

    public async Task DeleteWish(Guid wishId)
    {
        logger.LogInformation($"Deleting wish {wishId} by Child");
        await wishRepository.DeleteWishByWishId(wishId);
        logger.LogInformation(
            $"Successfully deleted wish {wishId} by Child");
    }

    #endregion

    #region Penalties

    public async Task<List<Penalty>> GetAllPenaltiesByChildId(Guid childId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all penalties by ChildId: {childId}");
        var penalties = await penaltyRepository.GetAllPenaltiesByChildId(childId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all penalties by ChildId : {childId}");
        return penalties;
    }

    #endregion
}