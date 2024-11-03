using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories;
using FamilyMerchandise.Function.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FamilyMerchandise.Function.Services;

public class ParentService(
    IAssignmentRepository assignmentRepository,
    IStepRepository stepRepository,
    IWishRepository wishRepository,
    IAchievementRepository achievementRepository,
    IPenaltyRepository penaltyRepository,
    ILogger<ParentService> logger)
    : IParentService
{
    #region Assignments

    public async Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId)
    {
        logger.LogInformation($"Getting all assignments by HomeId: {homeId}");
        var assignments = await assignmentRepository.GetAllAssignmentsByHomeId(homeId);

        foreach (var assignment in assignments)
        {
            logger.LogInformation($"Getting Children Info with HomeId: {homeId}");
            var steps = await stepRepository.GetAllStepsByAssignmentId(assignment.Id);
            assignment.SetSteps(steps);
        }

        logger.LogInformation(
            $"Successfully getting all assignments by HomeId : {homeId}");
        return assignments;
    }

    public async Task<Guid> CreateAssignment(CreateAssignmentRequest request)
    {
        logger.LogInformation($"Adding a new Assignment to Home: {request.HomeId}");
        var assignmentId = await assignmentRepository.InsertAssignment(request);
        logger.LogInformation(
            $"Successfully added an Assignment : {assignmentId}, by Parent {request.ParentId} to Child {request.ChildId}");
        return assignmentId;
    }

    public Task EditAssignment(Guid assignmentId, Assignment assignment)
    {
        throw new NotImplementedException();
    }

    public Task CompleteAssignment(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> CreateStepToAssignment(CreateStepRequest request)
    {
        logger.LogInformation($"Adding a new Step to Assignment: {request.AssignmentId}");
        var stepId = await stepRepository.InsertStep(request);
        logger.LogInformation(
            $"Successfully added a Atep : {stepId}, to Assignment {request.AssignmentId}");
        return stepId;
    }

    public Task EditStep(Guid stepId)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Wishes

    public async Task<List<Wish>> GetAllWishesByHomeId(Guid homeId)
    {
        logger.LogInformation($"Getting all wishes by HomeId: {homeId}");
        var wishes = await wishRepository.GetAllWishesByHomeId(homeId);
        logger.LogInformation(
            $"Successfully getting all wishes by HomeId : {homeId}");
        return wishes;
    }

    public Task EditWishCost(Guid wishId)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Achievements

    public async Task<List<Achievement>> GetAllAchievementByHomeId(Guid homeId)
    {
        logger.LogInformation($"Getting all achievements by HomeId: {homeId}");
        var achievements = await achievementRepository.GetAllAchievementsByHomeId(homeId);
        logger.LogInformation(
            $"Successfully getting all achievements by HomeId : {homeId}");
        return achievements;
    }

    public async Task<Guid> CreateAchievement(CreateAchievementRequest request)
    {
        logger.LogInformation($"Adding a new Achievement to Home: {request.HomeId}");
        var assignmentId = await achievementRepository.InsertAchievement(request);
        logger.LogInformation(
            $"Successfully added an Achievement : {assignmentId}, by Parent {request.ParentId} to Child {request.ChildId}");
        return assignmentId;
    }

    public Task EditAchievement(Guid achievementId)
    {
        throw new NotImplementedException();
    }

    public Task GrantAchievementBonus(Guid achievementId)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Penalties

    public async Task<List<Penalty>> GetAllPenaltiesByHomeId(Guid homeId)
    {
        logger.LogInformation($"Getting all penalties by HomeId: {homeId}");
        var penalties = await penaltyRepository.GetAllPenaltiesByHomeId(homeId);
        logger.LogInformation(
            $"Successfully getting all penalties by HomeId : {homeId}");
        return penalties;
    }

    public async Task<Guid> CreatePenalty(CreatePenaltyRequest request)
    {
        logger.LogInformation($"Adding a new Penalty to Home: {request.HomeId}");
        var penaltyId = await penaltyRepository.InsertPenalty(request);
        logger.LogInformation(
            $"Successfully added a Penalty : {penaltyId}, by Parent {request.ParentId} to Child {request.ChildId}");
        return penaltyId;
    }

    #endregion
}