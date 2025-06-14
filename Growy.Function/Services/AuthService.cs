using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;
using Growy.Function.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class AuthService(
    IAppUserRepository appUserRepository,
    IHomeRepository homeRepository,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<Guid> RegisterUser(AppUser user)
    {
        logger.LogInformation($"Registering a user to the app with email: {user.Email}");
        var userId = await appUserRepository.InsertIfNotExist(user);
        logger.LogInformation($"Successfully registered user with Id: {userId}");
        return userId;
    }

    public async Task<IActionResult> SecureExecute(HttpRequest req, Guid homeId, Func<Task<IActionResult>> func)
    {
        logger.LogInformation("Performing Authentication Check");

        if (homeId == Guid.Empty)
        {
            return new NotFoundObjectResult("No HomeId found, Check authentication failed");
        }

        try
        {
            var oid = GetIdpIdFromToken(req);
            // System Specific App User Id
            AppUser user;
            try
            {
                user = await appUserRepository.GetAppUserByIdpId(oid);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, $"Failed to get user with ID {oid}");
                return new NotFoundObjectResult($"Failed to get user with ID {oid}");
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
                try
                {
                    return await func();
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    return new BadRequestObjectResult(e.Message);
                }
            }

            return new UnauthorizedObjectResult(
                $"No home ids owned by user is {user.Id} matches the requested home Id {homeId}");
        }
        catch (AuthenticationException e)
        {
            return new UnauthorizedObjectResult(e.Message);
        }
    }

    public string GetIdpIdFromToken(HttpRequest request)
    {
        if (request.Headers.TryGetValue("Authorization", out var authHeaders))
        {
            var token = authHeaders.FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                throw new AuthenticationException("Valid authorization token is missing");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var oid = jwt.Claims.FirstOrDefault(c => c.Type == "oid")?.Value;

            if (string.IsNullOrEmpty(oid))
            {
                throw new AuthenticationException("no oid found in jwt");
            }

            return oid;
        }

        throw new AuthenticationException("No Authentication Header found.");
    }

    public async Task<Guid> GetAppUserIdFromToken(HttpRequest req)
    {
        var oid = GetIdpIdFromToken(req);
        return (await appUserRepository.GetAppUserByIdpId(oid)).Id;
    }
}