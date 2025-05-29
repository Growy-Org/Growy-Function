using System.Data.Common;
using Growy.Function.Exceptions;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class HomeService(
    IHomeRepository homeRepository,
    IChildRepository childRepository,
    IParentRepository parentRepository,
    IAssignmentRepository assignmentRepository,
    IAchievementRepository achievementRepository,
    IWishRepository wishRepository,
    IPenaltyRepository penaltyRepository,
    IStepRepository stepRepository,
    ILogger<HomeService> logger)
    : IHomeService
{
    #region Home


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

    public async Task<Guid> CreateHome(CreateHomeRequest request)
    {
        logger.LogInformation($"Adding a new Home: {request.HomeName}");
        var homeId = await homeRepository.InsertHome(request);
        logger.LogInformation($"Successfully added a home: {homeId}");
        return homeId;
    }

    public async Task<Guid> EditHome(EditHomeRequest request)
    {
        logger.LogInformation($"Editing Home: {request.HomeId}");
        var homeId = await homeRepository.EditHomeByHomeId(request);
        logger.LogInformation($"Successfully edit home : {homeId}");
        return homeId;
    }

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

    #region Children

    public async Task<Guid> AddChildToHome(Guid homeId, Child child)
    {
        logger.LogInformation($"Adding a new Child to Home: {homeId}");
        var childId = await childRepository.InsertChild(homeId, child);
        logger.LogInformation($"Successfully added a child : {childId} to Home: {homeId}");
        return childId;
    }

    public async Task<Guid> EditChild(EditChildRequest request)
    {
        logger.LogInformation($"Editing Child: {request.ChildId}");
        var childId = await childRepository.EditChildByChildId(request);
        logger.LogInformation($"Successfully edit child : {childId}");
        return childId;
    }

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

    #region Parents

    public async Task<Guid> AddParentToHome(Guid homeId, Parent parent)
    {
        logger.LogInformation($"Adding a new Parent to Home: {homeId}");
        var parentId = await parentRepository.InsertParent(homeId, parent);
        logger.LogInformation($"Successfully added a parent : {parentId} to Home: {homeId}");
        return parentId;
    }

    public async Task<Guid> EditParent(EditParentRequest request)
    {
        logger.LogInformation($"Editing Parent: {request.ParentId}");
        var parentId = await parentRepository.EditParentByParentId(request);
        logger.LogInformation($"Successfully edit parent : {parentId}");
        return parentId;
    }

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

    #region Assignments

    public async Task<Assignment> GetAssignmentById(Guid assignmentId)
    {
        logger.LogInformation($"Getting assignment by Id: {assignmentId}");
        var assignment = await assignmentRepository.GetAssignmentById(assignmentId);

        logger.LogInformation($"Getting Steps Info with assignment: {assignment.Id}");
        var steps = await stepRepository.GetAllStepsByAssignmentId(assignment.Id);
        assignment.SetSteps(steps);

        logger.LogInformation(
            $"Successfully getting assignment by Id: {assignment.Id}");
        return assignment;
    }

    public async Task<List<Assignment>> GetAllAssignmentsByHomeId(Guid homeId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all assignments by Home: {homeId}");
        var assignments = await assignmentRepository.GetAllAssignmentsByHomeId(homeId, pageNumber, pageSize);

        foreach (var assignment in assignments)
        {
            logger.LogInformation($"Getting Steps Info with assignment: {assignment.Id}");
            var steps = await stepRepository.GetAllStepsByAssignmentId(assignment.Id);
            assignment.SetSteps(steps);
        }

        logger.LogInformation(
            $"Successfully getting all assignments by Home : {homeId}");
        return assignments;
    }

    #endregion

    #region Wishes

    public async Task<List<Wish>> GetAllWishesByHomeId(Guid homeId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all wishes by Home: {homeId}");
        var wishes = await wishRepository.GetAllWishesByHomeId(homeId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all wishes by Home : {homeId}");
        return wishes;
    }

    public async Task<Wish> GetWishById(Guid wishId)
    {
        logger.LogInformation($"Getting wish by Id: {wishId}");
        var wish = await wishRepository.GetWishById(wishId);

        logger.LogInformation(
            $"Successfully getting wish by Id: {wish.Id}");
        return wish;
    }

    #endregion

    #region Achievements

    public async Task<List<Achievement>> GetAllAchievementByHomeId(Guid homeId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all achievements by HomeId: {homeId}");
        var achievements = await achievementRepository.GetAllAchievementsByHomeId(homeId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all achievements by HomeId : {homeId}");
        return achievements;
    }

    public async Task<Achievement> GetAchievementById(Guid achievementId)
    {
        logger.LogInformation($"Getting achievement by Id: {achievementId}");
        var achievement = await achievementRepository.GetAchievementById(achievementId);

        logger.LogInformation(
            $"Successfully getting achievement by Id: {achievement.Id}");
        return achievement;
    }

    #endregion

    #region Penalties

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

    #endregion
}