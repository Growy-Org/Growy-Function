using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class AppUserService(
    IAppUserRepository appUserRepository,
    IHomeRepository homeRepository,
    ILogger<HomeService> logger)
    : IAppUserService
{
    public async Task<Guid> RegisterUser(AppUser user)
    {
        logger.LogInformation($"Registering a user to the app with email: {user.Email}");
        var userId = await appUserRepository.InsertIfNotExist(user);
        logger.LogInformation($"Successfully registered user with Id: {userId}");
        return userId;
    }
    
    public async Task<List<Home>> GetHomesByAppUserId(Guid appUserId)
    {
        logger.LogInformation($"Getting home all homes by app user Id {appUserId}");
        var homes = await homeRepository.GetAllHomeByAppUserId(appUserId);
        logger.LogInformation($"Successfully get {homes.Count} by app user Id: {appUserId}");
        return homes;
    }

}