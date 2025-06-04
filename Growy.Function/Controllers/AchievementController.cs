using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Controllers;

public class AchievementController(
    ILogger<AchievementController> logger,
    IAchievementService achievementService)
{
    // Read
    [Function("GetAllAchievementsByParent")]
    public async Task<IActionResult> GetAllAchievementsByParent(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "parent/{id}/achievements")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var parentId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await achievementService.GetAllAchievementsByParentId(parentId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }


    [Function("GetAllAchievementsByChild")]
    public async Task<IActionResult> GetAllAchievementsByChild(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "child/{id}/achievements")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var childId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await achievementService.GetAllAchievementsByChildId(childId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    [Function("GetAllAchievementsByHome")]
    public async Task<IActionResult> GetAllAchievementsByHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/achievements")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await achievementService.GetAllAchievementsByHomeId(homeId,
            pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
            pageSize ?? Constants.DEFAULT_PAGE_SIZE);
        return new OkObjectResult(res);
    }

    [Function("GetAchievementById")]
    public async Task<IActionResult> GetAchievementById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "achievement/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var achievementId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await achievementService.GetAchievementById(achievementId);
        return new OkObjectResult(res);
    }

    // Create
    [Function("CreateAchievement")]
    public async Task<IActionResult> CreateAchievement(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/achievement")]
        HttpRequest req, [FromBody] CreateAchievementRequest achievementRequest)
    {
        var res = await achievementService.CreateAchievement(achievementRequest);
        return new OkObjectResult(res);
    }


    // Update
    [Function("EditAchievement")]
    public async Task<IActionResult> EditAchievement(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "achievement")]
        HttpRequest req, [FromBody] EditAchievementRequest request)
    {
        var res = await achievementService.EditAchievement(request);
        return new OkObjectResult(res);
    }


    [Function("GrantedAchievement")]
    public async Task<IActionResult> GrantedAchievement(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "achievement/{id}/grant")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var achievementId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await achievementService.EditAchievementGrants(achievementId, true);
        return new OkObjectResult(res);
    }

    [Function("RevokeGrantedAchievement")]
    public async Task<IActionResult> RevokeGrantedAchievement(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "achievement/{id}/revoke-grant")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var achievementId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await achievementService.EditAchievementGrants(achievementId, false);
        return new OkObjectResult(res);
    }

    // Delete
    [Function("DeleteAchievement")]
    public async Task<IActionResult> DeleteAchievement(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "achievement/{id}")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var achievementId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        await achievementService.DeleteAchievement(achievementId);
        return new OkResult();
    }
}