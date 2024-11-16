using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories;
using FamilyMerchandise.Function.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FamilyMerchandise.Function.Services;

public class ChildService(
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

    public async Task<List<Assignment>> GetAllAssignmentsByChildId(Guid childId)
    {
        logger.LogInformation($"Getting all assignments by ChildId: {childId}");
        var assignments = await assignmentRepository.GetAllAssignmentsByChildId(childId);

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



    public async Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId)
    {
        logger.LogInformation($"Getting all achievements by ChildId: {childId}");
        var achievements = await achievementRepository.GetAllAchievementsByChildId(childId);
        logger.LogInformation(
            $"Successfully getting all achievements by ChildId : {childId}");
        return achievements;
    }

    #endregion

    #region Wishes

    public async Task<List<Wish>> GetAllWishesByChildId(Guid childId)
    {
        logger.LogInformation($"Getting all wishes by ChildId: {childId}");
        var wishes = await wishRepository.GetAllWishesByChildId(childId);
        logger.LogInformation(
            $"Successfully getting all wishes by ChildId : {childId}");
        return wishes;
    }

    public async Task<Guid> CreateWish(CreateWishRequest request)
    {
        logger.LogInformation($"Adding a new Assignment to Home: {request.HomeId}");
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
            $"Successfully wish edited {request.WishId}");
        return id;
    }

    #endregion

    #region Penalties

    public async Task<List<Penalty>> GetAllPenaltiesByChildId(Guid childId)
    {
        logger.LogInformation($"Getting all penalties by ChildId: {childId}");
        var penalties = await penaltyRepository.GetAllPenaltiesByChildId(childId);
        logger.LogInformation(
            $"Successfully getting all penalties by ChildId : {childId}");
        return penalties;
    }

    #endregion
}