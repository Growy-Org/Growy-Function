using Dapper;
using Growy.Function.Entities;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;

namespace Growy.Function.Repositories;

public class AppUserRepository(IConnectionFactory connectionFactory) : IAppUserRepository
{
    private const string AppUsersTable = "appusers";

    public async Task<AppUser> GetAppUserByIdpId(string idpId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"SELECT * FROM {AppUsersTable} WHERE IdpId = @IdpId";
        var appUser = await con.QuerySingleAsync<AppUserEntity>(query, new { IdpId = idpId });
        return appUser.ToAppUser();
    }

    public async Task<Guid> InsertIfNotExist(AppUser user)
    {
        var appUserEntity = user.ToAppUserEntity();
        using var con = connectionFactory.GetDBConnection();
        // This will be called multiple time, we could just check if Idp id exist, because Idp Id should be Unique. Id is internal to this app only
        var query =
            $"INSERT INTO {AppUsersTable} (Email, IdentityProvider, IdpId, Sku, DisplayName) VALUES (@Email, @IdentityProvider, @IdpId, @Sku, @DisplayName) ON CONFLICT (IdpId) DO UPDATE SET IdpId = @IdpId RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, appUserEntity);
    }
}