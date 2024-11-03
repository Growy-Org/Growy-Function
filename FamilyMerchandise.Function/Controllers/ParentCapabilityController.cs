using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace FamilyMerchandise.Function.Controllers;

public class ParentCapabilityController(
    ILogger<ParentCapabilityController> logger,
    IParentService parentService)
{
    #region Assignments

    [Function("GetAllAssignmentsByHome")]
    public async Task<IActionResult> GetAllAssignmentsByHome(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "home/{id}/assignments")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.GetAllAssignmentsByHomeId(homeId);
        return new OkObjectResult(res);
    }

    [Function("CreateAssignment")]
    public async Task<IActionResult> CreateAssignmentToHome(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/assignment")]
        HttpRequest req, [FromBody] CreateAssignmentRequest assignmentRequest)
    {
        var res = await parentService.CreateAssignment(assignmentRequest);
        return new OkObjectResult(res);
    }

    [Function("CreateStep")]
    public async Task<IActionResult> CreateStep(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/step")]
        HttpRequest req, [FromBody] CreateStepRequest stepRequest)
    {
        var res = await parentService.CreateStepToAssignment(stepRequest);
        return new OkObjectResult(res);
    }

    #endregion

    #region Achivements

    [Function("GetAllAchievementsByHome")]
    public async Task<IActionResult> GetAllAchievementsByHome(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "home/{id}/achievements")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.GetAllAchievementByHomeId(homeId);
        return new OkObjectResult(res);
    }

    [Function("CreateAchievement")]
    public async Task<IActionResult> CreateAchievement(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/achievement")]
        HttpRequest req, [FromBody] CreateAchievementRequest achievementRequest)
    {
        var res = await parentService.CreateAchievement(achievementRequest);
        return new OkObjectResult(res);
    }

    #endregion

    #region Wishes

    [Function("GetAllWishesByHome")]
    public async Task<IActionResult> GetAllWishesByHome(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "home/{id}/wishes")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.GetAllWishesByHomeId(homeId);
        return new OkObjectResult(res);
    }

    #endregion

    #region Penalties

    [Function("GetAllPenaltiesByHome")]
    public async Task<IActionResult> GetAllPenaltiesByHome(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "home/{id}/penalties")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.GetAllPenaltiesByHomeId(homeId);
        return new OkObjectResult(res);
    }

    [Function("CreatePenalty")]
    public async Task<IActionResult> CreatePenalty(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/penalty")]
        HttpRequest req, [FromBody] CreatePenaltyRequest penaltyRequest)
    {
        var res = await parentService.CreatePenalty(penaltyRequest);
        return new OkObjectResult(res);
    }

    #endregion
}