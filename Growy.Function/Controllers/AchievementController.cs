using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace Growy.Function.Controllers;

public class AchievementController(
    IAchievementService achievementService,
    IParentService parentService,
    IChildService childService,
    IAuthService authService)
{
    // Read
    [Function("GetAchievementsCount")]
    public async Task<IActionResult> GetAchievementsCount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/achievements/count")]
        HttpRequest req, string id, [FromQuery] string? parentId, [FromQuery] string? childId,
        [FromQuery] string? showOnlyNotAchieved)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        if (!bool.TryParse(showOnlyNotAchieved, out var showOnlyNotAchievedBool))
        {
            showOnlyNotAchievedBool = false;
        }

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(err);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await parentId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await achievementService.GetAchievementsCount(homeId, parentIdGuid, childIdGuid,
                showOnlyNotAchievedBool);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllAchievements")]
    public async Task<IActionResult> GetAllAchievements(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/achievements")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize,
        [FromQuery] string? parentId, [FromQuery] string? childId, [FromQuery] string? showOnlyNotAchieved)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        if (!bool.TryParse(showOnlyNotAchieved, out var showOnlyNotAchievedBool))
        {
            showOnlyNotAchievedBool = false;
        }

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(err);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await parentId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await achievementService.GetAllAchievements(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE, parentIdGuid, childIdGuid
                , showOnlyNotAchievedBool);
            return new OkObjectResult(res);
        });
    }

    // Create
    [Function("CreateAchievement")]
    public async Task<IActionResult> CreateAchievement(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/achievement")]
        HttpRequest req, string id, [FromBody] AchievementRequest achievementRequest)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await achievementService.CreateAchievement(homeId, achievementRequest);
            return new OkObjectResult(res);
        });
    }

    // Update
    [Function("EditAchievement")]
    public async Task<IActionResult> EditAchievement(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "achievement/{id}")]
        HttpRequest req, string id, [FromBody] AchievementRequest request)
    {
        var (err, achievementId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await achievementService.GetHomeIdByAchievementId(achievementId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await achievementService.EditAchievement(achievementId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("GrantedAchievement")]
    public async Task<IActionResult> GrantedAchievement(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "achievement/{id}/grant")]
        HttpRequest req, string id)
    {
        var (err, achievementId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await achievementService.GetHomeIdByAchievementId(achievementId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await achievementService.EditAchievementGrants(achievementId, true);
            return new OkObjectResult(res);
        });
    }

    [Function("RevokeGrantedAchievement")]
    public async Task<IActionResult> RevokeGrantedAchievement(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "achievement/{id}/revoke-grant")]
        HttpRequest req, string id)
    {
        var (err, achievementId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await achievementService.GetHomeIdByAchievementId(achievementId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await achievementService.EditAchievementGrants(achievementId, false);
            return new OkObjectResult(res);
        });
    }

    // Delete
    [Function("DeleteAchievement")]
    public async Task<IActionResult> DeleteAchievement(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "achievement/{id}")]
        HttpRequest req, string id)
    {
        var (err, achievementId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await achievementService.GetHomeIdByAchievementId(achievementId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            await achievementService.DeleteAchievement(achievementId);
            return new NoContentResult();
        });
    }
}