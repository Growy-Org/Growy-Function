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

public class AppUserController(
    IAuthService authService)
{
    private const string MsAuthIdp = "MS-AZURE-ENTRA";

    [Function("SecureHomePing")]
    [OpenApiOperation(
        operationId: "SecureHomePing",
        tags: new[] { "Health" },
        Summary = "Secure health check",
        Description = "Health check endpoint required authorisation")]
    [OpenApiParameter(
        name: "id",
        In = ParameterLocation.Path,
        Required = true,
        Type = typeof(string),
        Summary = "The ID of the Home to ping",
        Description = "Home Id")]
    [OpenApiSecurity(
        "bearer_auth",
        SecuritySchemeType.Http,
        Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(string),
        Description = "Headers of the request")]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.BadRequest,
        Description = "Invalid home id")]
    public async Task<IActionResult> SecurePing(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "secure-home-ping/{id}")]
        HttpRequest req, string id)
    {
        var (err, homeId) = id.VerifyId();
        if (err != string.Empty) return new BadRequestObjectResult(err);

        return await authService.SecureExecute(req, homeId, async () =>
        {
            return await Task.FromResult(new OkObjectResult(string.Join("\n", req.Headers.Select(
                header => $"{header.Key}={string.Join(", ", header.Value)}"))));
        });
    }

    [Function("Ping")]
    [OpenApiOperation(
        operationId: "Ping",
        tags: new[] { "Health" },
        Summary = "Health check",
        Description = "Health check endpoint")]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(string),
        Description = "Headers of the request")]
    public IActionResult Ping(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ping")]
        HttpRequest req)
    {
        return new OkObjectResult(string.Join("\n", req.Headers.Select(
            header => $"{header.Key}={string.Join(", ", header.Value)}")));
    }

    [Function("RegisterAppUser")]
    [OpenApiOperation(operationId: "RegisterAppUser", tags: new[] { "App User" }, Summary = "Register app user",
        Description = "This endpoint register a app user with idempotency")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(AppUserRequest), Required = true,
        Description = "The details to create")]
    [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer,
        BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Guid),
        Description = "App User Guid Id")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Unauthorized,
        Description = "Not authorized to perform this action")]
    public async Task<IActionResult> RegisterAppUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "app-user")]
        HttpRequest req,
        [FromBody] AppUserRequest appUserRequest)
    {
        // Currently MS Azure Entra is used
        // Identity provider id == Object ID
        // Id == Object ID @ b2c
        // Id should only be meaningful to the system internally
        // Sku set as "Free" for now.
        var idpId = authService.GetIdpIdFromToken(req);
        var res = await authService.RegisterUser(new AppUser()
        {
            IdentityProvider = MsAuthIdp,
            IdpId = idpId,
            Email = appUserRequest.Email,
            Sku = AppSku.Free,
            DisplayName = appUserRequest.DisplayName
        });
        return new OkObjectResult(res);
    }
}