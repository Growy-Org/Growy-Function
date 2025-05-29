using Growy.Function.Models;

namespace Growy.Function.Services;

public interface IAppUserService
{
    public Task<Guid> RegisterUser(AppUser appUser);
}