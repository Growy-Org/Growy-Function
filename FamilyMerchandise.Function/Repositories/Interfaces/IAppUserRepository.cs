using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IAppUserRepository
{
    public Task<Guid> RegisterUser(AppUser user);
}