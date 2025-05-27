using Growy.Function.Models;

namespace Growy.Function.Services;

public interface IAppUserService
{
    public Task<AppUser> GetAppUserById(Guid appUserId);
    public Task<Guid> RegisterUser(AppUser appUser);
    public Task<Guid?> GetHomeIdByAppUserId(Guid appUserId);
}