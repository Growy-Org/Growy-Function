using System.Diagnostics;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FamilyMerchandise.Function.Services;

public class AnalyticService(
    IChildRepository childRepository,
    IAssignmentRepository assignmentRepository,
    IStepRepository stepRepository,
    IWishRepository wishRepository,
    IAchievementRepository achievementRepository,
    IPenaltyRepository penaltyRepository,
    IAnalyticRepository analyticRepository,
    ILogger<AnalyticService> logger)
    : IAnalyticService
{
    #region Assignments

    public async Task<AnalyticProfileResult<ParentAnalyticProfile>> GetLiveParentAnalyticProfile(
        ParentAnalyticViewType viewType, string? homeId, string? parentId, string? childId,
        int? year)
    {
        var result = new AnalyticProfileResult<ParentAnalyticProfile>
            { Status = RequestStatus.Success, Message = "Success!" };
        switch (viewType)
        {
            case ParentAnalyticViewType.AllParentsToAllChildren:

                if (!Guid.TryParse(homeId, out var homeIdGuid))
                {
                    logger.LogWarning($"Invalid homeId format: {homeId}, for viewType {viewType.ToString()}.");
                    result.Status = RequestStatus.Failure;
                    result.Message =
                        $"Invalid homeId format: {homeId}, Please ensure homeId is provided correctly in query param for viewType {viewType.ToString()}.";
                }
                else
                {
                    result.Result = await analyticRepository.GetAllParentsToAllChildAnalytic(homeIdGuid, year ?? DateTime.Now.Year);
                }

                break;
            case ParentAnalyticViewType.AllParentsToOneChild:
                break;
            case ParentAnalyticViewType.OneParentToOneChild:
                break;
        }

        return result;
    }

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
}