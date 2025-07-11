using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Growy.Function.Controllers;

public class AnalyticController(
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
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

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
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var (childErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId, true);
        if (childErr != string.Empty) return new BadRequestObjectResult(childErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var result =
                await analyticService.GetAllParentsToOneChildAnalyticLive(homeId, year, childIdGuid ?? Guid.Empty);
            return new OkObjectResult(result);
        });
    }
}