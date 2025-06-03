using System.IdentityModel.Tokens.Jwt;
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
    public async Task<IActionResult> SecureExecute(HttpRequest req, Guid homeId, Func<Task<IActionResult>> func)
    {
        logger.LogInformation("Performing Authentication Check");
        var oid = "";
        if (req.Headers.TryGetValue("Authorization", out var authHeaders))
        {
            var token = authHeaders.FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return new UnauthorizedObjectResult("Valid authorization token is missing");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            oid = jwt.Claims.FirstOrDefault(c => c.Type == "oid")?.Value;
        }

        // At the moment using MS Idp. Could use other Idp in the future
        if (string.IsNullOrEmpty(oid))
        {
            return new UnauthorizedObjectResult("No oid found from jwt, Authentication failed");
        }

        if (homeId == Guid.Empty)
        {
            return new NotFoundObjectResult("No HomeId found, Check authentication failed");
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

        var homes = await homeRepository.GetAllHomeByAppUserId(user.Id);
        if (homes.Count == 0)
        {
            logger.LogWarning($"No home found for app user ID {user.Id.ToString()}");
            return new NotFoundObjectResult($"Failed to get homeId with user ID {user.Id.ToString()}");
        }

        // Check to see if stored home id is matching the provided home id
        if (homes.Any(home => home.Id == homeId))
        {
            // Execute func method
            return await func();
        }

        return new UnauthorizedObjectResult(
            $"No home ids owned by user is {user.Id} matches the requested home Id {homeId}");
    }
}