using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Services.Interfaces;
using Growy.Function.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Growy.Function.Controllers;

public class AppUserController(
    IAuthService authService)
{
    private const string MsAuthIdp = "MS-AZURE-ENTRA";

    [Function("SecurePing")]
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
    public IActionResult Ping(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ping")]
        HttpRequest req)
    {
        return new OkObjectResult(string.Join("\n", req.Headers.Select(
            header => $"{header.Key}={string.Join(", ", header.Value)}")));
    }

    [Function("RegisterAppUser")]
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