using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class AuthWrapper(
    IAppUserRepository appUserRepository,
    IHomeRepository homeRepository,
    ILogger<AuthWrapper> logger) : IAuthWrapper
{
    private const string
        OidHeaderKey = "X-MS-CLIENT-PRINCIPAL-ID"; // Provided by Function App After authenticated 

    public async Task<IActionResult> SecureExecute(HttpRequest req, Guid homeId, Func<Task<IActionResult>> func)
    {
        logger.LogInformation("Performing Authentication Check");
        // At the moment using MS Idp. Could use other Idp in the future
        if (!req.Headers.TryGetValue(OidHeaderKey, out var oid))
        {
            return new UnauthorizedObjectResult($"No OID {OidHeaderKey} found, Authentication failed");
        }

        if (homeId == Guid.Empty)
        {
            return new NotFoundObjectResult($"No HomeId found, Check authentication failed");
        }

        // System Specific App User Id
        AppUser user;
        try
        {
            user = await appUserRepository.GetAppUserByIdpId(oid.ToString());
        }
        catch (Exception e)
        {
            logger.LogWarning(e, $"Failed to get user with ID {oid.ToString()}");
            return new NotFoundObjectResult($"Failed to get user with ID {oid.ToString()}");
        }

        var matchingHomeId = await homeRepository.GetHomeIdByAppUserId(user.Id);
        if (matchingHomeId == null)
        {
            logger.LogWarning($"Failed to get homeId with user ID {user.Id.ToString()}");
            return new NotFoundObjectResult($"Failed to get homeId with user ID {user.Id.ToString()}");
        }

        // Check to see if stored home id is matching the provided home id
        if (matchingHomeId.ToString() != homeId.ToString())
        {
            return new UnauthorizedObjectResult(
                $"HomeId found by {user.Id} is {homeId}, does not match homeId {matchingHomeId} in request header");
        }

        // Execute func method
        return await func();
    }
}