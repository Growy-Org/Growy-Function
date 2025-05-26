using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class AppUserCapabilityController(
    ILogger<AppUserCapabilityController> logger,
    IAppUserService appUserService)
{
    private const string AuthIdp = "MS-AZURE-B2C";

    [Function("SecurePing")]
    public IActionResult SecurePing(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "secure-ping")]
        HttpRequest req)
    {
        return new OkObjectResult(string.Join("\n", req.Headers.Select(
            header => $"{header.Key}={string.Join(", ", header.Value)}")));
    }

    [Function("GetAppUser")]
    public async Task<IActionResult> GetAppUser(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "app-user/{id}")]
        HttpRequest req,
        string id)
    {
        if (!Guid.TryParse(id, out var appUserId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await appUserService.GetAppUserById(appUserId);
        return new OkObjectResult(res);
    }

    [Function("RegisterAppUser")]
    public async Task<IActionResult> RegisterAppUser(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "app-user")]
        HttpRequest req,
        [FromBody] CreateAppUserRequest appUserRequest)
    {
        // Currently MS Azure B2C is used
        // Identity provider id == Object ID
        // Id == Object ID @ b2c
        // Id should only be meaningful to the system internally
        // Sku set as "Free" for now.
        var res = await appUserService.RegisterUser(new AppUser()
        {
            Id = Guid.Parse(appUserRequest.IdpId),
            IdentityProvider = AuthIdp,
            IdpId = appUserRequest.IdpId,
            Email = appUserRequest.Email,
            Sku = AppSku.Premium,
        });
        return new OkObjectResult(res);
    }

    [Function("GetHomeIdByAppUserId")]
    public async Task<IActionResult> GetHomeIdByAppUserId(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "app-user/{id}/home")]
        HttpRequest req,
        string id)
    {
        if (!Guid.TryParse(id, out var appUserId))
        {
            logger.LogWarning($"Invalid ID format: {id}");
            return new BadRequestObjectResult("Invalid ID format. Please provide a valid GUID.");
        }

        var res = await appUserService.GetHomeIdByAppUserId(appUserId);
        if (res == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(res);
    }
}