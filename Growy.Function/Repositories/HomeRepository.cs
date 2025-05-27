using Dapper;
using Growy.Function.Entities;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;
using Growy.Function.Repositories.Interfaces;

namespace Growy.Function.Repositories;

public class HomeRepository(IConnectionFactory connectionFactory) : IHomeRepository
{
    private const string HomesTable = "inventory.homes";

    public async Task<Home> GetHome(Guid homeId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"SELECT * FROM {HomesTable} WHERE Id = @Id";
        var homeEntity = await con.QuerySingleAsync<HomeEntity>(query, new { Id = homeId });
        return homeEntity.ToHome();
    }

    public async Task<Guid> InsertHome(CreateHomeRequest request)
    {
        var homeEntity = request.ToHomeEntity();
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {HomesTable} (Name, Address, AppUserId) VALUES (@Name, @Address, @AppUserId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, homeEntity);
    }

    public async Task<Guid> EditHomeByHomeId(EditHomeRequest request)
    {
        var homeEntity = request.ToHomeEntity();
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"UPDATE {HomesTable} SET Name = @Name, Address = @Address WHERE Id = @Id RETURNING Id;";
        return await con.ExecuteScalarAsync<Guid>(query, homeEntity);
    }

    public async Task DeleteHomeByHomeId(Guid homeId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"DELETE FROM {HomesTable} WHERE Id = @Id RETURNING Id;";
        await con.ExecuteScalarAsync<Guid>(query, new { Id = homeId });
    }

    public async Task<Guid> GetHomeIdByAppUserId(Guid appUserId)
    {
        using var con = connectionFactory.GetDBConnection();
        var query =
            $"SELECT Id FROM {HomesTable} WHERE AppUserId = @AppUserId";
        return await con.QuerySingleAsync<Guid>(query, new { AppUserId = appUserId });
    }
}