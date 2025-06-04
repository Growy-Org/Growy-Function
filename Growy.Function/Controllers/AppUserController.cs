using System.Security.Claims;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class AppUserController(
    ILogger<AppUserController> logger,
    IAuthService authService)
{
    private const string MsAuthIdp = "MS-AZURE-ENTRA";
    private const string HomeIdKey = "X-HOME-ID";

    [Function("SecurePing")]
    public async Task<IActionResult> SecurePing(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "secure-ping")]
        HttpRequest req, ClaimsPrincipal principal)
    {
        if (!req.Headers.TryGetValue(HomeIdKey, out var homeId))
        {
            return new UnauthorizedObjectResult($"No {HomeIdKey} found in header, Authentication failed");
        }

        return await authService.SecureExecute(req, Guid.Parse(homeId.ToString()), async () =>
        {
            return await Task.FromResult(new OkObjectResult(string.Join("\n", req.Headers.Select(
                header => $"{header.Key}={string.Join(", ", header.Value)}"))));
        });
    }

    [Function("Ping")]
    public IActionResult Ping(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ping")]
        HttpRequest req, ClaimsPrincipal principal)
    {
        return new OkObjectResult(string.Join("\n", req.Headers.Select(
            header => $"{header.Key}={string.Join(", ", header.Value)}")));
    }

    [Function("RegisterAppUser")]
    public async Task<IActionResult> RegisterAppUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "app-user")]
        HttpRequest req,
        [FromBody] CreateAppUserRequest appUserRequest)
    {
        // Currently MS Azure Entra is used
        // Identity provider id == Object ID
        // Id == Object ID @ b2c
        // Id should only be meaningful to the system internally
        // Sku set as "Free" for now.
        var res = await authService.RegisterUser(new AppUser()
        {
            IdentityProvider = MsAuthIdp,
            IdpId = appUserRequest.IdpId,
            Email = appUserRequest.Email,
            Sku = AppSku.Free,
            DisplayName = appUserRequest.DisplayName
        });
        return new OkObjectResult(res);
    }
}