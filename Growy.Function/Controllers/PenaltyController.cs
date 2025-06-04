using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Growy.Function.Controllers;

public class PenaltyController(
    ILogger<PenaltyController> logger,
    IPenaltyService penaltyService,
    IParentService parentService,
    IChildService childService,
    IAuthService authService)
{
    // Read
    [Function("GetAllPenaltiesByParent")]
    public async Task<IActionResult> GetAllPenaltiesByParent(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "parent/{id}/penalties")]
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
            var res = await penaltyService.GetAllPenaltiesByParentId(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllPenaltiesByChild")]
    public async Task<IActionResult> GetAlPenaltiesByChild(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "child/{id}/penalties")]
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
            var res = await penaltyService.GetAllPenaltiesByChildId(childId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllPenaltiesByHome")]
    public async Task<IActionResult> GetAllPenaltiesByHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/penalties")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await penaltyService.GetAllPenaltiesByHomeId(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
            return new OkObjectResult(res);
        });
    }

    [Function("GetPenaltyById")]
    public async Task<IActionResult> GetPenaltyById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "penalty/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var penaltyId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var homeId = await penaltyService.GetHomeIdByPenaltyId(penaltyId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await penaltyService.GetPenaltyById(penaltyId);
            return new OkObjectResult(res);
        });
    }

    // Create
    [Function("CreatePenalty")]
    public async Task<IActionResult> CreatePenalty(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/penalty")]
        HttpRequest req, string id, [FromBody] PenaltyRequest penaltyRequest)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await penaltyService.CreatePenalty(homeId, penaltyRequest);
            return new OkObjectResult(res);
        });
    }

    // Update
    [Function("EditPenalty")]
    public async Task<IActionResult> EditPenalty(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "penalty/{id}")]
        HttpRequest req, string id, [FromBody] PenaltyRequest request)
    {
        if (!Guid.TryParse(id, out var penaltyId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var homeId = await penaltyService.GetHomeIdByPenaltyId(penaltyId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await penaltyService.EditPenalty(penaltyId, request);
            return new OkObjectResult(res);
        });
    }

    // Delete
    [Function("DeletePenalty")]
    public async Task<IActionResult> DeletePenalty(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "penalty/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var penaltyId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var homeId = await penaltyService.GetHomeIdByPenaltyId(penaltyId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            await penaltyService.DeletePenalty(penaltyId);
            return new NoContentResult();
        });
    }
}