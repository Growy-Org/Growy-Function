using FamilyMerchandise.Function.Exceptions;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace FamilyMerchandise.Function.Controllers;

public class AppUserCapabilityController(
    ILogger<AppUserCapabilityController> logger,
    IAppUserService appUserService)
{
    private const string AuthIdp = "MS-AZURE-B2C";

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
        var res = await appUserService.RegisterUser(new AppUser()
        {
            Id = Guid.Parse(appUserRequest.IdpId),
            IdentityProvider = AuthIdp,
            IdpId = appUserRequest.IdpId,
            Email = appUserRequest.Email,
        });
        return new OkObjectResult(res);
    }
}