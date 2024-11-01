using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace FamilyMerchandise.Function.Controllers;

public class FunctionController(
    ILogger<FunctionController> logger,
    IHomeService homeService,
    IParentService parentService,
    IChildService childService)
{
    [Function("GetHome")]
    public async Task<IActionResult> GetHome(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "home/{id}")]
        HttpRequest req,
        string id)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await homeService.GetHomeInfoById(homeId);
        return new OkObjectResult(res);
    }

    [Function("AddHome")]
    public async Task<IActionResult> AddHome(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home")]
        HttpRequest req, [FromBody] Home home)
    {
        var res = await homeService.CreateHome(home);
        return new OkObjectResult(res);
    }

    [Function("AddChildToHome")]
    public async Task<IActionResult> AddChild(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/{id}/child")]
        HttpRequest req,
        string id, [FromBody] Child child)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await homeService.AddChildToHome(homeId, child);
        return new OkObjectResult(res);
    }

    [Function("AddParentToHome")]
    public async Task<IActionResult> AddParent(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/{id}/parent")]
        HttpRequest req,
        string id, [FromBody] Parent parent)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await homeService.AddParentToHome(homeId, parent);
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

    [Function("CreateWish")]
    public async Task<IActionResult> CreateWish(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/wish")]
        HttpRequest req, [FromBody] CreateWishRequest wishRequest)
    {
        var res = await childService.CreateWish(wishRequest);
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

    [Function("CreatePenalty")]
    public async Task<IActionResult> CreatePenalty(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "home/penalty")]
        HttpRequest req, [FromBody] CreateAssignmentRequest assignmentRequest)
    {
        var res = await parentService.CreateAssignment(assignmentRequest);
        return new OkObjectResult(res);
    }
}