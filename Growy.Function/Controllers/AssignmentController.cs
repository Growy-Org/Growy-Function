using Growy.Function.Exceptions;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Services;
using Growy.Function.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class AssignmentController(
    ILogger<AssignmentController> logger,
    IAssignmentService assignmentService,
    IAuthService authService)
{
    # region Assignments

    // Read
    [Function("GetAllAssignmentsByParent")]
    public async Task<IActionResult> GetAllAssignmentsByParent(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "parent/{id}/assignments")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var parentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await assignmentService.GetAllAssignmentsByParentId(parentId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    [Function("GetAllAssignmentsByChild")]
    public async Task<IActionResult> GetAllAssignmentsByChild(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "child/{id}/assignments")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var childId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await assignmentService.GetAllAssignmentsByChildId(childId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    [Function("GetAllAssignmentsByHome")]
    public async Task<IActionResult> GetAllAssignmentsByHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/assignments")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await assignmentService.GetAllAssignmentsByHomeId(homeId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    [Function("GetAssignmentById")]
    public async Task<IActionResult> GetAssignmentById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assignment/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await assignmentService.GetAssignmentById(assignmentId);
        return new OkObjectResult(res);
    }

    // Create
    [Function("CreateAssignment")]
    public async Task<IActionResult> CreateAssignmentToHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/assignment")]
        HttpRequest req, [FromBody] CreateAssignmentRequest assignmentRequest)
    {
        var res = await assignmentService.CreateAssignment(assignmentRequest);
        return new OkObjectResult(res);
    }

    // Update
    [Function("EditAssignment")]
    public async Task<IActionResult> EditAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "assignment")]
        HttpRequest req, [FromBody] EditAssignmentRequest request)
    {
        var res = await assignmentService.EditAssignment(request);
        return new OkObjectResult(res);
    }

    [Function("CompleteAssignment")]
    public async Task<IActionResult> CompleteAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "assignment/{id}/complete")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await assignmentService.EditAssignmentCompleteStatus(assignmentId, true);
        return new OkObjectResult(res);
    }

    [Function("UnCompleteAssignment")]
    public async Task<IActionResult> UnCompleteAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "assignment/{id}/incomplete")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await assignmentService.EditAssignmentCompleteStatus(assignmentId, false);
        return new OkObjectResult(res);
    }

    // Delete
    [Function("DeleteAssignment")]
    public async Task<IActionResult> DeleteAssignment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "assignment/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        await assignmentService.DeleteAssignment(assignmentId);
        return new OkResult();
    }

    #endregion

    #region Steps

    // Create
    [Function("CreateStep")]
    public async Task<IActionResult> CreateStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "step")]
        HttpRequest req, [FromBody] CreateStepRequest stepRequest)
    {
        var res = await assignmentService.CreateStepToAssignment(stepRequest);
        return new OkObjectResult(res);
    }

    // Update
    [Function("EditStep")]
    public async Task<IActionResult> EditStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "step")]
        HttpRequest req, [FromBody] EditStepRequest request)
    {
        var res = await assignmentService.EditStep(request);
        return new OkObjectResult(res);
    }

    [Function("CompleteStep")]
    public async Task<IActionResult> CompleteStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "step/{id}/complete")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var stepId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await assignmentService.EditStepCompleteStatus(stepId, true);
        return new OkObjectResult(res);
    }

    [Function("UnCompleteStep")]
    public async Task<IActionResult> UnCompleteStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "step/{id}/incomplete")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var stepId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await assignmentService.EditStepCompleteStatus(stepId, false);
        return new OkObjectResult(res);
    }

    // Delete
    [Function("DeleteStep")]
    public async Task<IActionResult> DeleteStep(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "step/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var stepId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        await assignmentService.DeleteStep(stepId);
        return new OkResult();
    }

    #endregion
}