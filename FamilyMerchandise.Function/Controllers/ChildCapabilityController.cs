using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace FamilyMerchandise.Function.Controllers;

public class ChildCapabilityController(
    ILogger<ChildCapabilityController> logger,
    IChildService childService)
{
    #region Assignments

    [Function("GetAllAssignmentsByChild")]
    public async Task<IActionResult> GetAllAssignmentsByChild(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "child/{id}/assignments")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var childId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await childService.GetAllAssignmentsByChildId(childId);
        return new OkObjectResult(res);
    }

    #endregion

    #region Achievements

    [Function("GetAllAchievementsByChild")]
    public async Task<IActionResult> GetAllAchievementsByHome(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "child/{id}/achievements")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var childId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await childService.GetAllAchievementsByChildId(childId);
        return new OkObjectResult(res);
    }

    #endregion

    #region Wishes

    [Function("GetAllWishesByChild")]
    public async Task<IActionResult> GetAllWishesByChild(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "child/{id}/wishes")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var childId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await childService.GetAllWishesByChildId(childId);
        return new OkObjectResult(res);
    }

    [Function("CreateWish")]
    public async Task<IActionResult> CreateWish(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/wish")]
        HttpRequest req, [FromBody] CreateWishRequest wishRequest)
    {
        var res = await childService.CreateWish(wishRequest);
        return new OkObjectResult(res);
    }

    #endregion

    #region Penalties

    [Function("GetAllPenaltiesByChild")]
    public async Task<IActionResult> GetAlPenaltiesByChild(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "child/{id}/penalties")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var childId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await childService.GetAllPenaltiesByChildId(childId);
        return new OkObjectResult(res);
    }

    #endregion
}