using System.Net;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class AchievementController(
    IAchievementService achievementService,
    IParentService parentService,
    IChildService childService,
    IAuthService authService)
{
    // Read
    [Function("GetAchievementsCount")]
    [OpenApiOperation(operationId: "GetAchievementCount", tags: new[] { "Achievement" },
        Summary = "Get Achievement Count",
        Description =
            "Retrieve the count of Achievements for a specific Home, optionally filtered by Parent ID, Child ID, or achieved status.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Home ID",
        Description = "The unique identifier of the home")]
    [OpenApiParameter(name: "parentId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Parent ID Filter",
        Description = "Optional parent ID to filter the Achievements")]
    [OpenApiParameter(name: "childId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Child ID Filter",
        Description = "Optional child ID to filter the Achievements")]
    [OpenApiParameter(name: "showOnlyNotAchieved", In = ParameterLocation.Query, Required = false,
        Type = typeof(bool),
        Summary = "Not Achieved Filter",
        Description = "Optional flag to show only not achieved Achievements (e.g., 'true')")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(int),
        Summary = "Achievements Count",
        Description = "Returns the count of Achievements matching the filter criteria")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
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
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await achievementService.GetAchievementsCount(homeId, parentIdGuid, childIdGuid,
                showOnlyNotAchievedBool);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllAchievements")]
    [OpenApiOperation(operationId: "GetAllAchievements", tags: new[] { "Achievement" },
        Summary = "Get All Achievements",
        Description =
            "Retrieve all Achievement records for a specific Home, optionally filtered by Parent ID, Child ID, and incomplete status.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Home ID",
        Description = "The unique identifier of the Home")]
    [OpenApiParameter(name: "pageNumber", In = ParameterLocation.Query, Required = false, Type = typeof(int),
        Summary = "Page Number",
        Description = "Optional page number for pagination")]
    [OpenApiParameter(name: "pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(int),
        Summary = "Page Size",
        Description = "Optional page size for pagination")]
    [OpenApiParameter(name: "parentId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Parent ID Filter",
        Description = "Optional Parent ID to filter the Achievements")]
    [OpenApiParameter(name: "childId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Child ID Filter",
        Description = "Optional Child ID to filter the Achievements")]
    [OpenApiParameter(name: "showOnlyNotAchieved", In = ParameterLocation.Query, Required = false,
        Type = typeof(bool),
        Summary = "Not Achieved Filter",
        Description = "Optional flag to show only not achieved Achievements (e.g., 'true')")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(List<Achievement>),
        Summary = "Achievement List Response",
        Description = "Returns the list of Achievements matching the specified filters")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
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
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

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
    [OpenApiOperation(operationId: "CreateAchievement", tags: new[] { "Achievement" },
        Summary = "Create Achievement",
        Description = "Create a new Achievement record for a specific Home.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Home ID",
        Description = "The unique identifier of the Home")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(AchievementRequest), Required = true,
        Description = "The Achievement object to be created")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(Guid),
        Summary = "Achievement Created",
        Description = "Returns the ID of the created Achievement")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
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
    [OpenApiOperation(operationId: "EditAchievement", tags: new[] { "Achievement" },
        Summary = "Edit Achievement",
        Description = "Update an existing Achievement record by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Achievement ID",
        Description = "The unique identifier of the Achievement to update")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(AchievementRequest), Required = true,
        Description = "The updated Achievement object")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Achievement Updated",
        Description = "The Achievement was successfully updated")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
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

    [Function("GrantAchievement")]
    [OpenApiOperation(operationId: "GrantAchievement", tags: new[] { "Achievement" },
        Summary = "Complete Achievement",
        Description = "Mark an Achievement as granted by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Achievement ID",
        Description = "The unique identifier of the Achievement to grant")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(Guid),
        Summary = "Achievement Granted",
        Description = "Returns the ID of the Achievement")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> GrantAchievement(
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
    [OpenApiOperation(operationId: "RevokeAchievementGrant", tags: new[] { "Achievement" },
        Summary = "Revoke Achievement Grant",
        Description = "Mark an Achievement as Un granted by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Achievement ID",
        Description = "The unique identifier of the Achievement to revoke")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(Guid),
        Summary = "Achievement Revoked",
        Description = "Returns the ID of the Achievement")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
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
    [OpenApiOperation(operationId: "DeleteAchievement", tags: new[] { "Achievement" },
        Summary = "Delete Achievement",
        Description = "Delete an Achievement by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Achievement ID",
        Description = "The unique identifier of the Achievement to delete")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Achievement Deleted",
        Description = "The Achievement was successfully deleted")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Conflict,
        Description = "Achievement record could not be deleted due to related records not deleted")]
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