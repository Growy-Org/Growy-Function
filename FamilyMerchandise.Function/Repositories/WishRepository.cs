using Dapper;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public class WishRepository(IConnectionFactory connectionFactory) : IWishRepository
{
    public const string WishesTable = "inventory.wishes";

    public async Task<Guid> InsertWish(CreateWishRequest request)
    {
        var wishEntity = request.ToWishEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {WishesTable} (Name, HomeId, IconCode, Description, GenieId, WisherId) VALUES (@Name, @HomeId, @IconCode, @Description, @GenieId, @WisherId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, wishEntity);
    }
}