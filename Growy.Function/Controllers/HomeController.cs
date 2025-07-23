using System.Net;
using System.Security.Authentication;
using Growy.Function.Exceptions;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class HomeController(
    IHomeService homeService,
    IAuthService authService)
{
    // Read
    [Function("GetHome")]
    [OpenApiOperation(
        operationId: "GetHome",
        tags: new[] { "Home" },
        Summary = "Get a Home by ID",
        Description = "This endpoint retrieves a specific Home record by its GUID identifier")]
    [OpenApiParameter(
        name: "id",
        In = ParameterLocation.Path,
        Required = true,
        Type = typeof(string),
        Summary = "The ID of the Home to retrieve",
        Description = "Home Id")]
    [OpenApiSecurity(
        "bearer_auth",
        SecuritySchemeType.Http,
        Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(Home),
        Description = "The Home record corresponding to the provided ID")]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.BadRequest,
        Description = "Invalid Home id")]
    public async Task<IActionResult> GetHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}")]
        HttpRequest req,
        string id)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await homeService.GetHomeInfoById(homeId);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllHomesByAppUserId")]
    [OpenApiOperation(
        operationId: "GetHomesByAppUserId",
        tags: new[] { "Home" },
        Summary = "Get all Homes by App User ID",
        Description =
            "This endpoint returns a list of Homes records belonging to a given App User ID. App User ID is inferred in JWT")]
    [OpenApiSecurity(
        "bearer_auth",
        SecuritySchemeType.Http,
        Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(List<Home>),
        Description = "List of Homes")]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.Unauthorized,
        Description = "Invalid JWT token")]
    public async Task<IActionResult> GetHomesByAppUserId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "homes")]
        HttpRequest req)
    {
        try
        {
            var appUserId = await authService.GetAppUserIdFromToken(req);
            var res = await homeService.GetHomesByAppUserId(appUserId);
            return new OkObjectResult(res);
        }
        catch (AuthenticationException e)
        {
            return new UnauthorizedObjectResult(e.Message);
        }
    }

    // Create
    [Function("AddHome")]
    [OpenApiOperation(operationId: "AddHome", tags: new[] { "Home" }, Summary = "Add a new Home",
        Description = "This endpoint adds a new Home record")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(HomeRequest), Required = true,
        Description = "The Home details to create")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Guid),
        Description = "Home Guid Id")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> AddHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home")]
        HttpRequest req, [FromBody] HomeRequest request)
    {
        try
        {
            var appUserId = await authService.GetAppUserIdFromToken(req);
            var res = await homeService.CreateHome(appUserId, request);
            return new OkObjectResult(res);
        }
        catch (AuthenticationException e)
        {
            return new UnauthorizedObjectResult(e.Message);
        }
    }

    // Update

    [Function("EditHome")]
    [OpenApiOperation(operationId: "EditHome", tags: new[] { "Home" }, Summary = "Edit a Home",
        Description = "This endpoint edits a Home record")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(HomeRequest), Required = true,
        Description = "The Home details to edit")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Guid),
        Description = "Home Guid Id")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> EditHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "home/{id}")]
        HttpRequest req, string id, [FromBody] HomeRequest request)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId,
            async () =>
            {
                var res = await homeService.EditHome(homeId, request);
                return new OkObjectResult(res);
            }
        );
    }

    // Delete
    [Function("DeleteHome")]
    [OpenApiOperation(operationId: "DeleteHome", tags: new[] { "Home" }, Summary = "Delete a Home",
        Description = "This endpoint deletes a Home record")]
    [OpenApiParameter(
        name: "id",
        In = ParameterLocation.Path,
        Required = true,
        Type = typeof(string),
        Summary = "Home ID",
        Description = "The ID of the Home to delete ")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.NoContent,
        Description = "Successfully deleted the Home")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Conflict,
        Description = "Home record could not be deleted due to related records not deleted")]
    public async Task<IActionResult> DeleteHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "home/{id}")]
        HttpRequest req, string id)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            try
            {
                await homeService.DeleteHome(homeId);
            }
            catch (DeletionFailureException _)
            {
                return new ConflictObjectResult(
                    $"Failed to delete Home with ID {homeId}, make sure all linked resources are deleted first");
            }

            return new NoContentResult();
        });
    }
}