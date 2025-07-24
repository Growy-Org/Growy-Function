using System.Net;
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

public class WishController(
    IWishService wishService,
    IParentService parentService,
    IChildService childService,
    IAuthService authService)
{
    // Read
    [Function("GetWishesCount")]
    [OpenApiOperation(operationId: "GetWishCount", tags: new[] { "Wish" },
        Summary = "Get Wish Count",
        Description =
            "Retrieve the count of Wishes for a specific Home, optionally filtered by Parent ID, Child ID, or achieved status.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Home ID",
        Description = "The unique identifier of the home")]
    [OpenApiParameter(name: "parentId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Parent ID Filter",
        Description = "Optional parent ID to filter the Wishes")]
    [OpenApiParameter(name: "childId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Child ID Filter",
        Description = "Optional child ID to filter the Wishes")]
    [OpenApiParameter(name: "showOnlyNotFulfilled", In = ParameterLocation.Query, Required = false,
        Type = typeof(bool),
        Summary = "Not Fulfilled Filter",
        Description = "Optional flag to show only not fulfilled Wishes (e.g., 'true')")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(int),
        Summary = "Wishes Count",
        Description = "Returns the count of Wishes matching the filter criteria")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> GetWishesCount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/wishes/count")]
        HttpRequest req, string id, [FromQuery] string? parentId, [FromQuery] string? childId,
        [FromQuery] string? showOnlyNotFulfilled)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        if (!bool.TryParse(showOnlyNotFulfilled, out var showOnlyNotFulfilledBool))
        {
            showOnlyNotFulfilledBool = false;
        }

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.GetWishesCount(homeId, parentIdGuid, childIdGuid,
                showOnlyNotFulfilledBool);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllWishes")]
    [OpenApiOperation(operationId: "GetAllWishes", tags: new[] { "Wish" },
        Summary = "Get All Wishes",
        Description =
            "Retrieve all Wish records for a specific Home, optionally filtered by Parent ID, Child ID, and incomplete status.")]
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
        Description = "Optional Parent ID to filter the Wishes")]
    [OpenApiParameter(name: "childId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Child ID Filter",
        Description = "Optional Child ID to filter the Wishes")]
    [OpenApiParameter(name: "showOnlyNotFulfilled", In = ParameterLocation.Query, Required = false,
        Type = typeof(bool),
        Summary = "Not Fulfilled Filter",
        Description = "Optional flag to show only not fulfilled Wishes (e.g., 'true')")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(List<Wish>),
        Summary = "Wish List Response",
        Description = "Returns the list of Wishes matching the specified filters")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> GetAllWishes(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/wishes")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize,
        [FromQuery] string? parentId, [FromQuery] string? childId, [FromQuery] string? showOnlyNotFulfilled)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        if (!bool.TryParse(showOnlyNotFulfilled, out var showOnlyNotFulfilledBool))
        {
            showOnlyNotFulfilledBool = false;
        }

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.GetAllWishes(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE, parentIdGuid, childIdGuid
                , showOnlyNotFulfilledBool);
            return new OkObjectResult(res);
        });
    }


    // Create
    [Function("CreateWish")]
    [OpenApiOperation(operationId: "CreateWish", tags: new[] { "Wish" },
        Summary = "Create Wish",
        Description = "Create a new Wish record for a specific Home.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Home ID",
        Description = "The unique identifier of the Home")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(WishRequest), Required = true,
        Description = "The Wish object to be created")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(Guid),
        Summary = "Wish Created",
        Description = "Returns the ID of the created Wish")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> CreateWish(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/wish")]
        HttpRequest req, string id, [FromBody] WishRequest request)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.CreateWish(homeId, request);
            return new OkObjectResult(res);
        });
    }

    // Update
    [Function("EditWish")]
    [OpenApiOperation(operationId: "EditWish", tags: new[] { "Wish" },
        Summary = "Edit Wish",
        Description = "Update an existing Wish record by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Wish ID",
        Description = "The unique identifier of the Wish to update")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(WishRequest), Required = true,
        Description = "The updated Wish object")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Wish Updated",
        Description = "The Wish was successfully updated")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> EditWish(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "wish/{id}")]
        HttpRequest req, string id, [FromBody] WishRequest request)
    {
        var (err, wishId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await wishService.GetHomeIdByWishId(wishId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.EditWish(wishId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("FulFillWish")]
    [OpenApiOperation(operationId: "FulFillWish", tags: new[] { "Wish" },
        Summary = "Fulfill Wish",
        Description = "Mark an Wish as fulfilled by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Wish ID",
        Description = "The unique identifier of the Wish to fulfill")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(Guid),
        Summary = "Wish Fulfilled",
        Description = "Returns the ID of the Wish")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> FulFillWish(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "wish/{id}/fulfill")]
        HttpRequest req, string id)
    {
        var (err, wishId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await wishService.GetHomeIdByWishId(wishId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.SetWishFulFilled(wishId, true);
            return new OkObjectResult(res);
        });
    }

    [Function("UnFulFillWish")]
    [OpenApiOperation(operationId: "UnFulfillWishGrant", tags: new[] { "Wish" },
        Summary = "Revoke Wish Grant",
        Description = "Mark an Wish as Un granted by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Wish ID",
        Description = "The unique identifier of the Wish to revoke")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(Guid),
        Summary = "Wish UnFulfilled",
        Description = "Returns the ID of the Wish")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> UnFulFillWish(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "wish/{id}/unfulfill")]
        HttpRequest req, string id)
    {
        var (err, wishId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await wishService.GetHomeIdByWishId(wishId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await wishService.SetWishFulFilled(wishId, false);
            return new OkObjectResult(res);
        });
    }

    // Delete
    [Function("DeleteWish")]
    [OpenApiOperation(operationId: "DeleteWish", tags: new[] { "Wish" },
        Summary = "Delete Wish",
        Description = "Delete an Wish by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Wish ID",
        Description = "The unique identifier of the Wish to delete")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Wish Deleted",
        Description = "The Wish was successfully deleted")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Conflict,
        Description = "Wish record could not be deleted due to related records not deleted")]
    public async Task<IActionResult> DeleteWish(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "wish/{id}")]
        HttpRequest req, string id)
    {
        var (err, wishId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await wishService.GetHomeIdByWishId(wishId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            await wishService.DeleteWish(wishId);
            return new NoContentResult();
        });
    }
}