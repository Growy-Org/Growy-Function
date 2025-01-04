using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;
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

    public async Task<List<Assignment>> GetAllAssignmentsByParentId(Guid parentId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all assignments by Parent: {parentId}");
        var assignments = await assignmentRepository.GetAllAssignmentsByParentId(parentId, pageNumber, pageSize);

        foreach (var assignment in assignments)
        {
            logger.LogInformation($"Getting Steps Info with assignment: {assignment.Id}");
            var steps = await stepRepository.GetAllStepsByAssignmentId(assignment.Id);
            assignment.SetSteps(steps);
        }

        logger.LogInformation(
            $"Successfully getting all assignments by Parent : {parentId}");
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

    public async Task<Guid> EditAssignment(EditAssignmentRequest request)
    {
        logger.LogInformation($"Editing assignment: {request.AssignmentId}");
        var assignmentId = await assignmentRepository.EditAssignmentByAssignmentId(request);
        logger.LogInformation(
            $"Successfully edited an Assignment : {assignmentId}, by Parent {request.ParentId} to Child {request.ChildId}");
        return assignmentId;
    }

    public async Task<Guid> EditAssignmentCompleteStatus(Guid assignmentId, bool isCompleted)
    {
        logger.LogInformation($"Setting Assignment :{assignmentId} to {(isCompleted ? "Completed" : "In-Complete")}");
        var response = await assignmentRepository.EditAssignmentCompleteStatus(assignmentId, isCompleted);
        logger.LogInformation(
            $"Successfully Setting Assignment : {response.Id} completed status");
        var childId = await childRepository.EditPointsByChildId(response.ChildId,
            isCompleted ? response.Points : -response.Points);

        logger.LogInformation(
            $"Successfully {(isCompleted ? "adding" : "removing")} {response.Points} Points {(isCompleted ? "to" : "from")} child profile with id: {childId}");
        return assignmentId;
    }

    public async Task DeleteAssignment(Guid assignmentId)
    {
        logger.LogInformation($"Deleting assignment {assignmentId}");
        await assignmentRepository.DeleteAssignmentByAssignmentId(assignmentId);
        logger.LogInformation(
            $"Successfully deleted assignment {assignmentId}");
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

    public async Task<Guid> EditStepCompleteStatus(Guid stepId, bool isCompleted)
    {
        logger.LogInformation($"Setting Step :{stepId} to {(isCompleted ? "Completed" : "In-Complete")}");
        var id = await stepRepository.EditStepCompleteStatusByStepId(stepId, isCompleted);
        logger.LogInformation(
            $"Successfully setting step {id} completed status");
        return stepId;
    }

    public async Task DeleteStep(Guid stepId)
    {
        logger.LogInformation($"Deleting step {stepId}");
        await stepRepository.DeleteStepByStepId(stepId);
        logger.LogInformation(
            $"Successfully deleted step {stepId}");
    }

    #endregion

    #region Wishes

    public async Task<List<Wish>> GetAllWishesByParentId(Guid parentId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all wishes by Parent: {parentId}");
        var wishes = await wishRepository.GetAllWishesByParentId(parentId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all wishes by Parent : {parentId}");
        return wishes;
    }

    public async Task<Guid> CreateWish(CreateWishRequest request)
    {
        logger.LogInformation($"Adding a new Wish to Home: {request.HomeId} by Parent");
        var wishId = await wishRepository.InsertWish(request);
        logger.LogInformation(
            $"Successfully added a wish : {wishId}, by Parent {request.ParentId} to Child {request.ChildId}");
        return wishId;
    }
    
    public async Task<Guid> EditWish(EditWishRequest request)
    {
        logger.LogInformation($"Editing wish {request.WishId} by Parent");
        var id = await wishRepository.EditWishByWishId(request);
        logger.LogInformation(
            $"Successfully wish edited {request.WishId} by Parent");
        return id;
    }

    public async Task<Guid> SetWishFullFilled(Guid wishId, bool isFullFilled)
    {
        logger.LogInformation($"{(isFullFilled ? "Full Filling" : "Un Full Filling")} wish by Parent");
        var response =
            await wishRepository.EditWishFullFillStatusByWishId(wishId, isFullFilled);
        logger.LogInformation(
            $"Successfully edit full fill status with id: {response.Id} by Parent");

        var childId = await childRepository.EditPointsByChildId(response.ChildId,
            isFullFilled ? -response.Points : response.Points);

        logger.LogInformation(
            $"Successfully {(isFullFilled ? "reducing" : "adding")} {response.Points} Points {(isFullFilled ? "from" : "back")} child profile with id: {childId} by Parent");
        return response.Id;
    }

    public async Task DeleteWish(Guid wishId)
    {
        logger.LogInformation($"Deleting wish {wishId} by Parent");
        await wishRepository.DeleteWishByWishId(wishId);
        logger.LogInformation(
            $"Successfully deleted wish {wishId} by Parent");
    }
    #endregion

    #region Achievements

    public async Task<List<Achievement>> GetAllAchievementByParentId(Guid parentId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all achievements by Parent: {parentId}");
        var achievements = await achievementRepository.GetAllAchievementsByParentId(parentId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all achievements by Parent : {parentId}");
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
            $"Successfully {(isAchievementGranted ? "adding" : "removing")} {response.Points} Points {(isAchievementGranted ? "to" : "from")} child profile with id: {childId}");
        return response.Id;
    }

    public async Task DeleteAchievement(Guid achievementId)
    {
        logger.LogInformation($"Deleting achievement {achievementId}");
        await achievementRepository.DeleteAchievementByAchievementId(achievementId);
        logger.LogInformation(
            $"Successfully deleted achievement {achievementId}");
    }

    #endregion

    #region Penalties

    public async Task<List<Penalty>> GetAllPenaltiesByParentId(Guid parentId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all penalties by Parent: {parentId}");
        var penalties = await penaltyRepository.GetAllPenaltiesByParentId(parentId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all penalties by Parent : {parentId}");
        return penalties;
    }

    public async Task<Guid> CreatePenalty(CreatePenaltyRequest request)
    {
        logger.LogInformation($"Adding a new Penalty to Home: {request.HomeId}");
        var response = await penaltyRepository.InsertPenalty(request);
        var childId = await childRepository.EditPointsByChildId(response.ChildId, -response.Points);
        logger.LogInformation(
            $"Successfully reduce points from Child: {childId}");
        logger.LogInformation(
            $"Successfully added a Penalty : {response.Id}, Points deducted : {response.Points}, by Parent {request.ParentId} to Child {childId}");
        return response.Id;
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