using Growy.Function.Models.Dtos;
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
    IParentService parentService,
    IChildService childService,
    IAuthService authService)
{
    # region Assignments

    // Read

    [Function("GetAllAssignments")]
    public async Task<IActionResult> GetAllAssignments(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/assignments")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize,
        [FromQuery] string? parentId, [FromQuery] string? childId)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        // Parent Id Validation
        Guid? parentIdGuid = null;
        if (!string.IsNullOrEmpty(parentId))
        {
            if (Guid.TryParse(parentId, out var parentGuid))
            {
                parentIdGuid = parentGuid;
                var parentHomeId = await parentService.GetHomeIdByParentId(parentGuid);
                if (parentHomeId != homeId)
                {
                    return new BadRequestObjectResult($"parent {parentId} does not belongs to the home {homeId}");
                }
            }
            else
            {
                logger.LogWarning($"Invalid Parent ID format: {parentId}");
                return new BadRequestObjectResult($"Invalid parent ID format {parentId}. Please provide a valid GUID.");
            }

        }

        // Parent Id Validation
        Guid? childIdGuid = null;
        if (!string.IsNullOrEmpty(childId))
        {
            if (Guid.TryParse(childId, out var childGuid))
            {
                childIdGuid = childGuid;
                var childHomeId = await childService.GetHomeIdByChildId(childGuid);
                if (childHomeId != homeId)
                {
                    return new BadRequestObjectResult($"childId {childId} does not belongs to the home {homeId}");
                }
            }
            else
            {
                logger.LogWarning($"Invalid Child ID format: {childId}");
                return new BadRequestObjectResult($"Invalid child ID format {childId}. Please provide a valid GUID.");
            }
        }
        
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.GetAllAssignments(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE, parentIdGuid, childIdGuid);
            return new OkObjectResult(res);
        });
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

        var homeId = await assignmentService.GetHomeIdByAssignmentId(assignmentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.GetAssignmentById(assignmentId);
            return new OkObjectResult(res);
        });
    }

    // Create
    [Function("CreateAssignment")]
    public async Task<IActionResult> CreateAssignmentToHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/assignment")]
        HttpRequest req, string id, [FromBody] AssignmentRequest assignmentRequest)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

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
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

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
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

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
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

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
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

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
        if (!Guid.TryParse(id, out var assignmentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

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
        if (!Guid.TryParse(id, out var stepId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

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
        if (!Guid.TryParse(id, out var stepId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

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
        if (!Guid.TryParse(id, out var stepId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

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
        if (!Guid.TryParse(id, out var stepId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var homeId = await assignmentService.GetHomeIdByStepId(stepId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            await assignmentService.DeleteStep(stepId);
            return new NoContentResult();
        });
    }

    #endregion
}