using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class AssignmentController(
    IAssignmentService assignmentService,
    IParentService parentService,
    IChildService childService,
    IAuthService authService)
{
    # region Assignments

    // Read
    [Function("GetAssignmentCount")]
    public async Task<IActionResult> GetAssignmentCount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/assignments/count")]
        HttpRequest req, string id, [FromQuery] string? parentId, [FromQuery] string? childId,
        [FromQuery] string? showOnlyIncomplete)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        if (!bool.TryParse(showOnlyIncomplete, out var showOnlyIncompleteBool))
        {
            showOnlyIncompleteBool = false;
        }

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.GetAssignmentsCount(homeId, parentIdGuid, childIdGuid,
                showOnlyIncompleteBool);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllAssignments")]
    public async Task<IActionResult> GetAllAssignments(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/assignments")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize,
        [FromQuery] string? parentId, [FromQuery] string? childId, [FromQuery] string? showOnlyIncomplete)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        if (!bool.TryParse(showOnlyIncomplete, out var showOnlyIncompleteBool))
        {
            showOnlyIncompleteBool = false;
        }

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.GetAllAssignments(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE, parentIdGuid, childIdGuid
                , showOnlyIncompleteBool);
            return new OkObjectResult(res);
        });
    }

    // Create
    [Function("CreateAssignment")]
    public async Task<IActionResult> CreateAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/assignment")]
        HttpRequest req, string id, [FromBody] AssignmentRequest assignmentRequest)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.CreateAssignment(homeId, assignmentRequest);
            return new OkObjectResult(res);
        });
    }

    // Update
    [Function("EditAssignment")]
    public async Task<IActionResult> EditAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "assignment/{id}")]
        HttpRequest req, string id, [FromBody] AssignmentRequest request)
    {
        var (err, assignmentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByAssignmentId(assignmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.EditAssignment(assignmentId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("CompleteAssignment")]
    public async Task<IActionResult> CompleteAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "assignment/{id}/complete")]
        HttpRequest req, string id)
    {
        var (err, assignmentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByAssignmentId(assignmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.EditAssignmentCompleteStatus(assignmentId, true);
            return new OkObjectResult(res);
        });
    }

    [Function("UnCompleteAssignment")]
    public async Task<IActionResult> UnCompleteAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "assignment/{id}/incomplete")]
        HttpRequest req, string id)
    {
        var (err, assignmentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByAssignmentId(assignmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.EditAssignmentCompleteStatus(assignmentId, false);
            return new OkObjectResult(res);
        });
    }

    // Delete
    [Function("DeleteAssignment")]
    public async Task<IActionResult> DeleteAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "assignment/{id}")]
        HttpRequest req, string id)
    {
        var (err, assignmentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByAssignmentId(assignmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            await assignmentService.DeleteAssignment(assignmentId);
            return new NoContentResult();
        });
    }

    #endregion

    #region Steps

    // Create
    [Function("CreateStep")]
    public async Task<IActionResult> CreateStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "assignment/{id}/step")]
        HttpRequest req, string id, [FromBody] StepRequest stepRequest)
    {
        var (err, assignmentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByAssignmentId(assignmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.CreateStepToAssignment(assignmentId, stepRequest);
            return new OkObjectResult(res);
        });
    }

    // Update
    [Function("EditStep")]
    public async Task<IActionResult> EditStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "step/{id}")]
        HttpRequest req, string id, [FromBody] StepRequest request)
    {
        var (err, stepId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByStepId(stepId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.EditStep(stepId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("CompleteStep")]
    public async Task<IActionResult> CompleteStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "step/{id}/complete")]
        HttpRequest req, string id)
    {
        var (err, stepId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByStepId(stepId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.EditStepCompleteStatus(stepId, true);
            return new OkObjectResult(res);
        });
    }

    [Function("UnCompleteStep")]
    public async Task<IActionResult> UnCompleteStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "step/{id}/incomplete")]
        HttpRequest req, string id)
    {
        var (err, stepId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByStepId(stepId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.EditStepCompleteStatus(stepId, false);
            return new OkObjectResult(res);
        });
    }

    // Delete
    [Function("DeleteStep")]
    public async Task<IActionResult> DeleteStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "step/{id}")]
        HttpRequest req, string id)
    {
        var (err, stepId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await assignmentService.GetHomeIdByStepId(stepId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            await assignmentService.DeleteStep(stepId);
            return new NoContentResult();
        });
    }

    #endregion
}