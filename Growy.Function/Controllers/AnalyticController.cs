using System.Net;
using Growy.Function.Models;
using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Growy.Function.Controllers;

public class AnalyticController(
    IAnalyticService analyticService,
    IChildService childService,
    IAuthService authService
)
{
    [Function("GetAnalyticsByHome")]
    [OpenApiOperation(operationId: "GetHomeAnalytics", tags: new[] { "Analytics" }, 
        Summary = "Get Home Analytics", 
        Description = "Retrieve analytics data for a given home for all children and optionally filter by year")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), 
        Summary = "Home ID", 
        Description = "The unique identifier of the home")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiParameter(name: "year", In = ParameterLocation.Query, Required = false, Type = typeof(int), 
        Summary = "Year filter", 
        Description = "Optional year to filter the analytics data")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", 
        bodyType: typeof(AnalyticProfile), 
        Summary = "Analytics response", 
        Description = "Returns the analytics data")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
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
    [OpenApiOperation(operationId: "GetChildAnalytics", tags: new[] { "Analytics" }, 
        Summary = "Get Child Analytics", 
        Description = "Retrieve analytics data for a one child and optionally filter by year")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), 
        Summary = "Child ID", 
        Description = "The unique identifier of the home")]
    [OpenApiParameter(name: "year", In = ParameterLocation.Query, Required = false, Type = typeof(int), 
        Summary = "Year filter", 
        Description = "Optional year to filter the analytics data")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", 
        bodyType: typeof(AnalyticProfile), 
        Summary = "Analytics response", 
        Description = "Returns the analytics data")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> GetChildAnalytics(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "analytic/child/{id}")]
        HttpRequest req, string id, [FromQuery] int? year)
    {
        var (err, childId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await childService.GetHomeIdByChildId(childId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var result =
                await analyticService.GetAllParentsToOneChildAnalyticLive(homeId, year, childId);
            return new OkObjectResult(result);
        });
    }
}