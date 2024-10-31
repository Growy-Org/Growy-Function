using Dapper;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public class HomeRepository(IConnectionFactory connectionFactory) : IHomeRepository
{
    public const string HOMES_TABLE = "inventory.homes";

    public async Task<Home> GetHome(Guid homeId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"SELECT * FROM {HOMES_TABLE} WHERE Id = @Id";
        var homeEntity = await con.QuerySingleAsync<HomeEntity>(query, new { Id = homeId });
        return homeEntity.ToHome();
    }

    public async Task<Guid> InsertHome(Home home)
    {
        var homeEntity = home.ToHomeEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {HOMES_TABLE} (Name) VALUES (@Name) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, home);
    }
}