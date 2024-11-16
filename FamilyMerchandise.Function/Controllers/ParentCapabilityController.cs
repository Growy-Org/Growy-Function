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
    
    
    [Function("EditStep")]
    public async Task<IActionResult> EditStep(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "step")]
        HttpRequest req, [FromBody] EditStepRequest request)
    {
        var res = await parentService.EditStep(request);
        return new OkObjectResult(res);
    }
    
    
    [Function("CompleteStep")]
    public async Task<IActionResult> CompleteStep(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "step/{id}/complete")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var stepId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.EditStepCompleteStatus(stepId, true);
        return new OkObjectResult(res);
    }

    [Function("UnCompleteStep")]
    public async Task<IActionResult> UnCompleteStep(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "step/{id}/incomplete")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var stepId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.EditStepCompleteStatus(stepId, false);
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

    [Function("EditAchievement")]
    public async Task<IActionResult> EditAchievement(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "achievement")]
        HttpRequest req, [FromBody] EditAchievementRequest request)
    {
        var res = await parentService.EditAchievement(request);
        return new OkObjectResult(res);
    }


    [Function("GrantedAchievement")]
    public async Task<IActionResult> GrantedAchievement(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "achievement/{id}/grant")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var achievementId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.EditAchievementGrants(achievementId, true);
        return new OkObjectResult(res);
    }

    [Function("RevokeGrantedAchievement")]
    public async Task<IActionResult> RevokeGrantedAchievement(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "achievement/{id}/revoke-grant")]
        HttpRequest req, string id)
    {
        if (!Guid.TryParse(id, out var achievementId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await parentService.EditAchievementGrants(achievementId, false);
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

    // can only edit points nothing else, UI controls the 
    [Function("EditWishFromParent")]
    public async Task<IActionResult> EditWishFromParent(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "parent/wish")]
        HttpRequest req, [FromBody] EditWishRequest request)
    {
        // this is the same from backend POV for now as the children
        // TODO: Add validation for the request
        var res = await parentService.EditWish(request);
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

    [Function("EditPenalty")]
    public async Task<IActionResult> EditPenalty(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "penalty")]
        HttpRequest req, [FromBody] EditPenaltyRequest request)
    {
        var res = await parentService.EditPenalty(request);
        return new OkObjectResult(res);
    }

    #endregion
}