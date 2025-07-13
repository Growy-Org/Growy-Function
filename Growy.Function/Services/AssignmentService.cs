using Growy.Function.Models.Dtos;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;
using Growy.Function.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class AssignmentService(
    IChildRepository childRepository,
    IAssignmentRepository assignmentRepository,
    IConnectionFactory connectionFactory,
    IStepRepository stepRepository,
    ILogger<AssignmentService> logger)
    : IAssignmentService
{
    # region Assignments

    // Read
    public async Task<int> GetAssignmentsCount(Guid homeId, Guid? parentId, Guid? childId,
        bool showOnlyIncomplete = false)
    {
        return await assignmentRepository.GetAssignmentsCount(homeId, parentId, childId, showOnlyIncomplete);
    }

    public async Task<Guid> GetHomeIdByAssignmentId(Guid assignmentId)
    {
        return await assignmentRepository.GetHomeIdByAssignmentId(assignmentId);
    }

    public async Task<Guid> GetHomeIdByStepId(Guid stepId)
    {
        var assignmentId = await stepRepository.GetAssignmentIdByStepId(stepId);
        return await assignmentRepository.GetHomeIdByAssignmentId(assignmentId);
    }

    public async Task<List<Assignment>> GetAllAssignments(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId, bool showOnlyIncomplete = false)
    {
        logger.LogInformation("Getting all assignments");
        var assignments =
            await assignmentRepository.GetAllAssignments(homeId, pageNumber, pageSize, parentId, childId,
                showOnlyIncomplete);

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


    // Create
    public async Task<Guid> CreateAssignment(Guid homeId, AssignmentRequest request)
    {
        logger.LogInformation($"Adding a new Assignment to Home: {homeId}");
        var assignmentId = await assignmentRepository.InsertAssignment(homeId, request);
        logger.LogInformation(
            $"Successfully added an Assignment : {assignmentId}, by Parent {request.ParentId} to Child {request.ChildId}");
        return assignmentId;
    }

    // Update
    public async Task<Guid> EditAssignment(Guid assignmentId, AssignmentRequest request)
    {
        logger.LogInformation($"Editing assignment: {assignmentId}");
        var id = await assignmentRepository.EditAssignmentByAssignmentId(assignmentId, request);
        logger.LogInformation(
            $"Successfully edited an Assignment : {assignmentId}, by Parent {request.ParentId} to Child {request.ChildId}");
        return id;
    }

    public async Task<Guid> EditAssignmentCompleteStatus(Guid assignmentId, bool isCompleted)
    {
        logger.LogInformation($"Setting Assignment :{assignmentId} to {(isCompleted ? "Completed" : "In-Complete")}");
        using var con = await connectionFactory.GetDBConnection();
        con.Open();
        using var transaction = con.BeginTransaction();
        var response =
            await assignmentRepository.EditAssignmentCompleteStatus(assignmentId, isCompleted, con, transaction);
        logger.LogInformation(
            $"Successfully Setting Assignment : {response.Id} completed status");
        var childId = await childRepository.EditPointsByChildId(response.ChildId,
            isCompleted ? response.Points : -response.Points, con, transaction);

        logger.LogInformation(
            $"Successfully {(isCompleted ? "adding" : "removing")} {response.Points} Points {(isCompleted ? "to" : "from")} child profile with id: {childId}");
        transaction.Commit();
        return assignmentId;
    }

    // Delete
    public async Task DeleteAssignment(Guid assignmentId)
    {
        logger.LogInformation($"Deleting assignment {assignmentId}");
        await assignmentRepository.DeleteAssignmentByAssignmentId(assignmentId);
        logger.LogInformation(
            $"Successfully deleted assignment {assignmentId}");
    }

    # endregion

    # region Steps

    // Create
    public async Task<Guid> CreateStepToAssignment(Guid assignmentId, StepRequest request)
    {
        logger.LogInformation($"Adding a new Step to Assignment: {assignmentId}");
        var id = await stepRepository.InsertStep(assignmentId, request);
        logger.LogInformation(
            $"Successfully added a Step : {id}, to Assignment {assignmentId}");
        return id;
    }

    // Update
    public async Task<Guid> EditStep(Guid stepId, StepRequest request)
    {
        logger.LogInformation($"Editing Step: {stepId}");
        var id = await stepRepository.EditStepByStepId(stepId, request);
        logger.LogInformation(
            $"Successfully edited Step: {id}");
        return id;
    }

    public async Task<Guid> EditStepCompleteStatus(Guid stepId, bool isCompleted)
    {
        logger.LogInformation($"Setting Step :{stepId} to {(isCompleted ? "Completed" : "In-Complete")}");
        var id = await stepRepository.EditStepCompleteStatusByStepId(stepId, isCompleted);
        logger.LogInformation(
            $"Successfully setting step {id} completed status");
        return stepId;
    }

    // Delete
    public async Task DeleteStep(Guid stepId)
    {
        logger.LogInformation($"Deleting step {stepId}");
        await stepRepository.DeleteStepByStepId(stepId);
        logger.LogInformation(
            $"Successfully deleted step {stepId}");
    }

    # endregion
}