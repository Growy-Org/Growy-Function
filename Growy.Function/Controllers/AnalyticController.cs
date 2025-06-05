using Growy.Function.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Growy.Function.Controllers;

public class AnalyticController(
    ILogger<AnalyticController> logger,
    IAnalyticService analyticService,
    IChildService childService,
    IAuthService authService
)
{
    [Function("GetHomeAnalytics")]
    public async Task<IActionResult> GetHomeAnalytics(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "analytic/home/{id}")]
        HttpRequest req, string id, [FromQuery] int? year)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var result = await analyticService.GetAllParentsToAllChildAnalyticLive(homeId, year);
            return new OkObjectResult(result);
        });
    }


    [Function("GetParentAnalytics")]
    public async Task<IActionResult> GetParentAnalytics(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "analytic/home/{id}/child/{id}")]
        HttpRequest req, string id, [FromQuery] int? year)
    {
        if (!Guid.TryParse(id, out var childId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var homeId = await childService.GetHomeIdByChildId(childId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var result = await analyticService.GetAllParentsToOneChildAnalyticLive(childId, year);
            return new OkObjectResult(result);
        });
    }

    [Function("GetChildAnalytics")]
    public async Task<IActionResult> GetChildAnalytics(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "analytic/child/{id}")]
        HttpRequest req, string id, [FromQuery] int? year)
    {
        if (!Guid.TryParse(id, out var childId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var homeId = await childService.GetHomeIdByChildId(childId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var result = await analyticService.GetChildAnalyticByChildIdLive(childId, year);
            return new OkObjectResult(result);
        });
    }
}