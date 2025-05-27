using Growy.Function.Models;

namespace Growy.Function.Repositories.Interfaces;

public interface IAppUserRepository
{
    public Task<AppUser> GetAppUserByIdpId(string oid);
    public Task<Guid> InsertIfNotExist(AppUser user);
}