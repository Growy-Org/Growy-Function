using Dapper;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

public class AppUserRepository(IConnectionFactory connectionFactory) : IAppUserRepository
{
    private const string AppUsersTable = "appusers";

    public async Task<Guid> RegisterUser(AppUser user)
    {
        var appUserEntity = user.ToAppUserEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {AppUsersTable} (Id, Email, IdentityProvider, IdpId) VALUES (@Id, @Email, @IdentityProvider, @IdpId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, appUserEntity);
    }
}