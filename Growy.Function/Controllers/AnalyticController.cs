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
    [Function("GetAnalyticsToAllChild")]
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


    [Function("GetAnalyticsToOneChild")]
    public async Task<IActionResult> GetParentAnalytics(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "analytic/home/{id}/child/{childId}")]
        HttpRequest req, string id, string childId, [FromQuery] int? year)
    {
        if (!Guid.TryParse(id, out var homeId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        if (!Guid.TryParse(childId, out var childIdGuid))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var homeIdFromChild = await childService.GetHomeIdByChildId(childIdGuid);
        if (homeId != homeIdFromChild)
        {
            return new BadRequestObjectResult($"child id {childIdGuid} provided does not belong to the home {homeId}");
        }

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var result = await analyticService.GetAllParentsToOneChildAnalyticLive(homeId, year, childIdGuid);
            return new OkObjectResult(result);
        });
    }
}