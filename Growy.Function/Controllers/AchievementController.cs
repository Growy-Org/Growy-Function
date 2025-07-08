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
    [Function("GetAllAchievementsByParent")]
    public async Task<IActionResult> GetAllAchievementsByParent(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "parent/{id}/achievements")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        var (err, parentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await parentService.GetHomeIdByParentId(parentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await achievementService.GetAllAchievementsByParentId(parentId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllAchievementsByChild")]
    public async Task<IActionResult> GetAllAchievementsByChild(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "child/{id}/achievements")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        var (err, childId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await childService.GetHomeIdByChildId(childId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await achievementService.GetAllAchievementsByChildId(childId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllAchievementsByHome")]
    public async Task<IActionResult> GetAllAchievementsByHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/achievements")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await achievementService.GetAllAchievementsByHomeId(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAchievementById")]
    public async Task<IActionResult> GetAchievementById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "achievement/{id}")]
        HttpRequest req, string id)
    {
        var (err, achievementId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await achievementService.GetHomeIdByAchievementId(achievementId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await achievementService.GetAchievementById(achievementId);
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