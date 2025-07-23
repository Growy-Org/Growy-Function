using System.Net;
using Growy.Function.Exceptions;
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

public class ParentController(
    IParentService parentService,
    IAuthService authService)
{
    [Function("AddParentToHome")]
    [OpenApiOperation(
        operationId: "AddParent",
        tags: new[] { "Parent" },
        Summary = "Add Parent To Home",
        Description = "This endpoint adds a parent record to a Home with Home Id")]
    [OpenApiParameter(
        name: "id",
        In = ParameterLocation.Path,
        Required = true,
        Type = typeof(string),
        Summary = "The ID of the Home to retrieve",
        Description = "Home Id")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ParentRequest), Required = true,
        Description = "The details to create")]
    [OpenApiSecurity(
        "bearer_auth",
        SecuritySchemeType.Http,
        Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Guid),
        Description = "Parent Guid Id")]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.BadRequest,
        Description = "Invalid Home ID")]
    public async Task<IActionResult> AddParentToHome(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "home/{id}/parent")]
        HttpRequest req,
        string id, [FromBody] ParentRequest request)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await parentService.AddParentToHome(homeId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("EditParent")]
    [OpenApiOperation(operationId: "EditParent", tags: new[] { "Parent" }, Summary = "Edit a Parent",
        Description = "This endpoint edits a Parent record")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ParentRequest), Required = true,
        Description = "The details to edit")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Guid),
        Description = "Parent Guid Id")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> EditParent(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "parent/{id}")]
        HttpRequest req, string id, [FromBody] ParentRequest request)
    {
        var (err, parentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await parentService.GetHomeIdByParentId(parentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            var res = await parentService.EditParent(parentId, request);
            return new OkObjectResult(res);
        });
    }

    [Function("DeleteParent")]
    [OpenApiOperation(
        operationId: "DeleteParent",
        tags: new[] { "Parent" },
        Summary = "Delete a Parent record",
        Description = "This endpoint deletes a parent record")]
    [OpenApiParameter(
        name: "id",
        In = ParameterLocation.Path,
        Required = true,
        Type = typeof(string),
        Summary = "Parent Id",
        Description = "Parent Id")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.NoContent,
        Description = "Successfully deleted the Parent")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Conflict,
        Description = "Parent record could not be deleted due to related records not deleted")]
    public async Task<IActionResult> DeleteParent(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "parent/{id}")]
        HttpRequest req, string id)
    {
        var (err, parentId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        var homeId = await parentService.GetHomeIdByParentId(parentId);
        return await authService.SecureExecute(req, homeId, async () =>
        {
            try
            {
                await parentService.DeleteParent(parentId);
            }
            catch (DeletionFailureException _)
            {
                return new ConflictObjectResult(
                    $"Failed to delete Parent with ID {parentId}, make sure all linked resources are deleted first");
            }

            return new NoContentResult();
        });
    }
}