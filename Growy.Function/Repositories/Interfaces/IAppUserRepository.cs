using Growy.Function.Models;

namespace Growy.Function.Repositories.Interfaces;

public interface IAppUserRepository
{
    public Task<AppUser> GetAppUserById(Guid appUserId);
    public Task<Guid> InsertIfNotExist(AppUser user);
}