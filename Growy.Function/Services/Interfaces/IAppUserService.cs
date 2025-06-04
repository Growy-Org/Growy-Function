using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IAppUserService
{
    // Create
    public Task<Guid> RegisterUser(AppUser appUser);
}