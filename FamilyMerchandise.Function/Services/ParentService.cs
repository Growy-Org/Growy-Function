using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories;
using FamilyMerchandise.Function.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FamilyMerchandise.Function.Services;

public class ParentService(
    IChildRepository childRepository,
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
            $"Successfully added a Step : {stepId}, to Assignment {request.AssignmentId}");
        return stepId;
    }

    public async Task<Guid> EditStep(EditStepRequest request)
    {
        logger.LogInformation($"Editing Step: {request.StepId}");
        var stepId = await stepRepository.EditStepByStepId(request);
        logger.LogInformation(
            $"Successfully edited Step: {stepId}");
        return stepId;
    }

    public async Task<Guid> EditStepCompleteStatus(Guid stepId, bool isComplete)
    {
        logger.LogInformation($"Setting Step :{stepId} to {(isComplete ? "Completed" : "In-Complete")}");
        var id = await stepRepository.EditStepCompleteStatusByStepId(stepId, isComplete);
        logger.LogInformation(
            $"Successfully setting step {id} completed status");
        return stepId;
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

    public async Task<Guid> EditWish(EditWishRequest request)
    {
        logger.LogInformation($"Editing wish {request.WishId} for parent");
        var id = await wishRepository.EditWishByWishId(request);
        logger.LogInformation(
            $"Successfully wish edited {request.WishId}");
        return id;
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

    public async Task<Guid> EditAchievement(EditAchievementRequest request)
    {
        logger.LogInformation($"Editing achievement {request.AchievementId}");
        var id = await achievementRepository.EditAchievementByAchievementId(request);
        logger.LogInformation(
            $"Successfully edit achievement with id: {id}");
        return id;
    }

    public async Task<Guid> EditAchievementGrants(Guid achievementId, bool isAchievementGranted)
    {
        logger.LogInformation($"{(isAchievementGranted ? "Granting" : "Revoking")} achievement bonus");
        var response =
            await achievementRepository.EditAchievementGrantByAchievementId(achievementId, isAchievementGranted);
        logger.LogInformation(
            $"Successfully edit achievement grant with id: {response.Id}");

        var childId = await childRepository.EditPointsByChildId(response.ChildId,
            isAchievementGranted ? response.Points : -response.Points);

        logger.LogInformation(
            $"Successfully {(isAchievementGranted ? "adding" : "removing")} {response.Points} {(isAchievementGranted ? "to" : "from")} child profile with id: {childId}");
        return response.Id;
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

    public async Task<Guid> EditPenalty(EditPenaltyRequest request)
    {
        logger.LogInformation($"Editing penalty {request.PenaltyId}");
        var response = await penaltyRepository.EditPenaltyByPenaltyId(request);
        // if two children are different, then just apply the change. Else, only make changes if points deducted has changed
        if (request.ChildId != response.OldChildId)
        {
            logger.LogInformation(
                $"Changes in two children's points detected, applying changes to child {request.ChildId} and reverting changes to {response.OldChildId}");

            // TODO: should make this a transactional request in the future.
            // deduct points from new child.
            var newChildId = await childRepository.EditPointsByChildId(request.ChildId, -request.PenaltyPointsDeducted);
            logger.LogInformation(
                $"Successfully apply point change to new child: {newChildId} deducted points: {request.PenaltyPointsDeducted}");

            // add back the points deducted
            var oldChildId = await childRepository.EditPointsByChildId(response.OldChildId, response.OldPointsDeducted);
            logger.LogInformation(
                $"Successfully apply point change to old child: {oldChildId} adding back points: {response.OldPointsDeducted}");
        }
        else if (request.PenaltyPointsDeducted - response.OldPointsDeducted != 0)
        {
            logger.LogInformation(
                $"Changes in one child's points detected, applying points change to child {request.ChildId}");

            // both old and new child id are the same, Formula: Final Child Points = Original + (OldPointsDeducted - NewPointsDeducted)
            var childId = await childRepository.EditPointsByChildId(request.ChildId,
                response.OldPointsDeducted - request.PenaltyPointsDeducted);
            logger.LogInformation(
                $"Successfully apply point change to child: {childId}");
        }

        return response.Id;
    }

    #endregion
}