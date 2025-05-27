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
        OID_HEADER_KEY = "X-MS-CLIENT-PRINCIPAL-ID"; // Provided by Function App After authenticated 

    private const string
        HOME_ID_KEY = "X-HOME-ID"; // Home ID for every request

    public async Task<IActionResult> SecureExecute(HttpRequest req, Func<Task<IActionResult>> func)
    {
        logger.LogInformation("Performing Authentication Check");
        // At the moment using MS Idp. Could use other Idp in the future
        if (!req.Headers.TryGetValue(OID_HEADER_KEY, out var oid))
        {
            return new UnauthorizedObjectResult($"No OID {OID_HEADER_KEY} found, Authentication failed");
        }

        if (!req.Headers.TryGetValue(HOME_ID_KEY, out var homeId))
        {
            return new UnauthorizedObjectResult($"No HomeId {HOME_ID_KEY} found, Check authentication failed");
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

        Guid matchingHomeId;
        try
        {
            matchingHomeId = await homeRepository.GetHomeIdByAppUserId(user.Id);
        }
        catch (Exception e)
        {
            logger.LogWarning(e, $"Failed to get homeId with user ID {user.Id.ToString()}");
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