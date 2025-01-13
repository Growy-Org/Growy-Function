using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyMerchandise.Function.Controllers;

public class AnalyticCapabilityController(
    ILogger<AnalyticCapabilityController> logger,
    IAnalyticService analyticService)
{
    #region Analtytics

    [Function("GetParentAnalytics")]
    public async Task<IActionResult> GetParentAnalytics(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "analytic/getParentAnalytics/{analyticType}")]
        HttpRequest req, string analyticType, [FromQuery] string? homeId,
        [FromQuery] string? parentId, [FromQuery] string? childId, [FromQuery] int? year)
    {
        if (!Enum.TryParse<ParentAnalyticViewType>(analyticType, out var viewType))
        {
            logger.LogWarning($"Invalid analyticType value: {analyticType}");
            return new BadRequestObjectResult(
                $" 'analyticType' is provided with invalid value: '{analyticType}', accepted values are : [{string.Join(", ", Enum.GetNames(typeof(ParentAnalyticViewType)))}]");
        }

        var analyticRes = await analyticService.GetLiveParentAnalyticProfile(viewType, homeId, parentId, childId, year);

        if (analyticRes.Status == RequestStatus.Failure)
        {
            return new BadRequestObjectResult($"Fail to retrieve parent analytics because {analyticRes.Message}");
        }

        return new OkObjectResult(analyticRes.Result);
    }

    # endregion
}