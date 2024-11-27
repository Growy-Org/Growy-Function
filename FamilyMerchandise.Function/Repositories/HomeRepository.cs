using Dapper;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

public class HomeRepository(IConnectionFactory connectionFactory) : IHomeRepository
{
    private const string HomesTable = "inventory.homes";

    public async Task<Home> GetHome(Guid homeId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"SELECT * FROM {HomesTable} WHERE Id = @Id";
        var homeEntity = await con.QuerySingleAsync<HomeEntity>(query, new { Id = homeId });
        return homeEntity.ToHome();
    }

    public async Task<Guid> InsertHome(CreateHomeRequest request)
    {
        var homeEntity = request.ToHomeEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {HomesTable} (Name, Address, AppUserId) VALUES (@Name, @Address, @AppUserId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, homeEntity);
    }

    public async Task<Guid> EditHomeByHomeId(EditHomeRequest request)
    {
        var homeEntity = request.ToHomeEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"UPDATE {HomesTable} SET Name = @Name, Address = @Address WHERE Id = @Id RETURNING Id;";
        return await con.ExecuteScalarAsync<Guid>(query, homeEntity);
    }

    public async Task DeleteHomeByHomeId(Guid homeId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"DELETE FROM {HomesTable} WHERE Id = @Id RETURNING Id;";
        await con.ExecuteScalarAsync<Guid>(query, new { Id = homeId });
    }
}