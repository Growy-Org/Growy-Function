using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FamilyMerchandise.Function.Services;

public class AppUserService(
    IAppUserRepository appUserRepository,
    ILogger<HomeService> logger)
    : IAppUserService
{
    public async Task<Guid> RegisterUser(AppUser user)
    {
        logger.LogInformation($"Registering a new user to the app with email: {user.Email}");
        var userId = await appUserRepository.RegisterUser(user);
        logger.LogInformation($"Successfully registered user with Id: {userId}");
        return userId;
    }
}