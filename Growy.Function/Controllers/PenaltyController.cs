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

public class PenaltyController(
    IPenaltyService penaltyService,
    IParentService parentService,
    IChildService childService,
    IAuthService authService)
{
    // Read
    [Function("GetPenaltiesCount")]
    [OpenApiOperation(operationId: "GetPenaltyCount", tags: new[] { "Penalty" },
        Summary = "Get Penalty Count",
        Description =
            "Retrieve the count of Penalties for a specific Home, optionally filtered by Parent ID, Child ID, or achieved status.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Home ID",
        Description = "The unique identifier of the home")]
    [OpenApiParameter(name: "parentId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Parent ID Filter",
        Description = "Optional parent ID to filter the Penalties")]
    [OpenApiParameter(name: "childId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Child ID Filter",
        Description = "Optional child ID to filter the Penalties")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(int),
        Summary = "Penalties Count",
        Description = "Returns the count of Penalties matching the filter criteria")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> GetPenaltiesCount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/penalties/count")]
        HttpRequest req, string id, [FromQuery] string? parentId, [FromQuery] string? childId)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await penaltyService.GetPenaltiesCount(homeId, parentIdGuid, childIdGuid);
            return new OkObjectResult(res);
        });
    }

    [Function("GetAllPenalties")]
    [OpenApiOperation(operationId: "GetAllPenalties", tags: new[] { "Penalty" },
        Summary = "Get All Penalties",
        Description =
            "Retrieve all Penalty records for a specific Home, optionally filtered by Parent ID, Child ID, and incomplete status.")]
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
        Description = "Optional Parent ID to filter the Penalties")]
    [OpenApiParameter(name: "childId", In = ParameterLocation.Query, Required = false, Type = typeof(string),
        Summary = "Child ID Filter",
        Description = "Optional Child ID to filter the Penalties")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(List<Penalty>),
        Summary = "Penalty List Response",
        Description = "Returns the list of Penalties matching the specified filters")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> GetAllPenalties(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home/{id}/penalties")]
        HttpRequest req, string id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize,
        [FromQuery] string? parentId, [FromQuery] string? childId)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        // Parent Id Validation
        var (parentIdErr, parentIdGuid) = await parentId.VerifyIdFromHome(homeId, parentService.GetHomeIdByParentId);
        if (parentIdErr != string.Empty) return new BadRequestObjectResult(parentIdErr);

        // Child Id Validation
        var (childIdErr, childIdGuid) = await childId.VerifyIdFromHome(homeId, childService.GetHomeIdByChildId);
        if (childIdErr != string.Empty) return new BadRequestObjectResult(childIdErr);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await penaltyService.GetAllPenalties(homeId,
                pageNumber ?? Constants.DEFAULT_PAGE_NUMBER,
                pageSize ?? Constants.DEFAULT_PAGE_SIZE, parentIdGuid, childIdGuid);
            return new OkObjectResult(res);
        });
    }

    // Create
    [Function("CreatePenalty")]
    [OpenApiOperation(operationId: "CreatePenalty", tags: new[] { "Penalty" },
        Summary = "Create Penalty",
        Description = "Create a new Penalty record for a specific Home.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Home ID",
        Description = "The unique identifier of the Home")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(PenaltyRequest), Required = true,
        Description = "The Penalty object to be created")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(Guid),
        Summary = "Penalty Created",
        Description = "Returns the ID of the created Penalty")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> CreatePenalty(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/penalty")]
        HttpRequest req, string id, [FromBody] PenaltyRequest penaltyRequest)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await penaltyService.CreatePenalty(homeId, penaltyRequest);
            return new OkObjectResult(res);
        });
    }

    // Update
    [Function("EditPenalty")]
    [OpenApiOperation(operationId: "EditPenalty", tags: new[] { "Penalty" },
        Summary = "Edit Penalty",
        Description = "Update an existing Penalty record by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Penalty ID",
        Description = "The unique identifier of the Penalty to update")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(PenaltyRequest), Required = true,
        Description = "The updated Penalty object")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
        bodyType: typeof(Guid),
        Summary = "Penalty Updated",
        Description = "Returns the ID of the Penalty")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> EditPenalty(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "penalty/{id}")]
        HttpRequest req, string id, [FromBody] PenaltyRequest request)
    {
        var (err, penaltyId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await penaltyService.GetHomeIdByPenaltyId(penaltyId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await penaltyService.EditPenalty(penaltyId, request);
            return new OkObjectResult(res);
        });
    }

    // Delete
    [Function("DeletePenalty")]
    [OpenApiOperation(operationId: "DeletePenalty", tags: new[] { "Penalty" },
        Summary = "Delete Penalty",
        Description = "Delete an Penalty by its ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string),
        Summary = "Penalty ID",
        Description = "The unique identifier of the Penalty to delete")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
        Summary = "Penalty Deleted",
        Description = "The Penalty was successfully deleted")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Conflict,
        Description = "Penalty record could not be deleted due to related records not deleted")]
    public async Task<IActionResult> DeletePenalty(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "penalty/{id}")]
        HttpRequest req, string id)
    {
        var (err, penaltyId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await penaltyService.GetHomeIdByPenaltyId(penaltyId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            await penaltyService.DeletePenalty(penaltyId);
            return new NoContentResult();
        });
    }
}