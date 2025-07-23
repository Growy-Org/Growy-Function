using System.Net;
using Growy.Function.Exceptions;
using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class ChildController(
    IChildService childService,
    IAuthService authService)
{
    [Function("AddChildToHome")]
    [OpenApiOperation(
        operationId: "AddChild",
        tags: new[] { "Child" },
        Summary = "Add Child To Home",
        Description = "This endpoint adds a Child record to a Home with Home Id")]
    [OpenApiParameter(
        name: "id",
        In = ParameterLocation.Path,
        Required = true,
        Type = typeof(string),
        Summary = "The ID of the Home to retrieve",
        Description = "Home Id")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ChildRequest), Required = true,
        Description = "The details to create")]
    [OpenApiSecurity(
        "bearer_auth",
        SecuritySchemeType.Http,
        Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Guid),
        Description = "Child Guid Id")]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.BadRequest,
        Description = "Invalid Home ID")]
    public async Task<IActionResult> AddChildToHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/child")]
        HttpRequest req,
        string id, [FromBody] ChildRequest request)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await childService.AddChildToHome(homeId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("EditChild")]
    [OpenApiOperation(operationId: "EditChild", tags: new[] { "Child" }, Summary = "Edit a Child",
        Description = "This endpoint edits a Child record")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ChildRequest), Required = true,
        Description = "The details to edit")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Guid),
        Description = "Child Guid Id")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> EditChild(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "child/{id}")]
        HttpRequest req, string id, [FromBody] ChildRequest request)
    {
        var (err, childId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await childService.GetHomeIdByChildId(childId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await childService.EditChild(childId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("DeleteChild")]
    [OpenApiOperation(
        operationId: "DeleteChild",
        tags: new[] { "Child" },
        Summary = "Delete a Child record",
        Description = "This endpoint deletes a Child record")]
    [OpenApiParameter(
        name: "id",
        In = ParameterLocation.Path,
        Required = true,
        Type = typeof(string),
        Summary = "Child Id",
        Description = "Child Id")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.NoContent,
        Description = "Successfully deleted the Child")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Conflict,
        Description = "Child record could not be deleted due to related records not deleted")]
    public async Task<IActionResult> DeleteChild(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "child/{id}")]
        HttpRequest req, string id)
    {
        var (err, childId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await childService.GetHomeIdByChildId(childId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            try
            {
                await childService.DeleteChild(childId);
            }
            catch (DeletionFailureException _)
            {
                return new ConflictObjectResult(
                    $"Failed to delete Child with ID {childId}, make sure all linked resources are deleted first");
            }

            return new NoContentResult();
        });
    }
}