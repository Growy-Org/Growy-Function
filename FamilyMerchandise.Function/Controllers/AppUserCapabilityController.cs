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
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "app-user")]
        HttpRequest req,
        [FromBody] AppUser appUser)
    {
        
        // Currently MS Azure B2C is used
        appUser.IdentityProvider = AuthIdp;
        // Id == Object ID @ b2c
        // Identity provider id == Object ID
        appUser.IdpId = appUser.Id.ToString();
        var res = await appUserService.RegisterUser(appUser);
        return new OkObjectResult(res);
    }
}