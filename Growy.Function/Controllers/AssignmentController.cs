using Growy.Function.Exceptions;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Repositories.Interfaces;
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
    IParentService parentService,
    IChildService childService,
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

        var homeId = await parentService.GetHomeIdByParentId(parentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.GetAllAssignmentsByParentId(parentId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
            return new OkObjectResult(res);
        });
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

        var homeId = await childService.GetHomeIdByChildId(childId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.GetAllAssignmentsByChildId(childId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
            return new OkObjectResult(res);
        });
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

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await assignmentService.GetAllAssignmentsByHomeId(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
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