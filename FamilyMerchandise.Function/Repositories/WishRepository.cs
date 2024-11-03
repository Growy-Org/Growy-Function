using Dapper;
using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Entities;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;

namespace FamilyMerchandise.Function.Repositories;

public class WishRepository(IConnectionFactory connectionFactory) : IWishRepository
{
    public const string WishesTable = "inventory.wishes";
    public const string ChildrenTable = "inventory.children";
    public const string ParentTable = "inventory.parents";

    public async Task<List<Wish>> GetAllWishesByHomeId(Guid homeId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            @$"
                SELECT 
                    w.Id, w.HomeId, w.Name, w.Description, w.IconCode, w.PointsCost, 
                    w.CreatedDateUtc, w.UpdatedDateUtc, w.FullFilledDateUtc,
                    c.Id, c.HomeId, c.Name, c.DOB, c.Gender, c.IconCode, c.PointsEarned,
                    p.Id, p.HomeId, p.Name, p.DOB, p.Role, p.IconCode
                FROM {WishesTable} w
                LEFT JOIN {ChildrenTable} c ON w.WisherId = c.Id
                LEFT JOIN {ParentTable} p ON w.GenieId = p.Id
                WHERE w.HomeId = @HomeId
            ";
        
        var wishEntities =
            await con.QueryAsync(query, _mapEntitiesToWishModel,
                new { HomeId = homeId });
        return wishEntities.ToList();
    }

    public async Task<List<Wish>> GetAllWishesByChildId(Guid childId)
    {
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            @$"
                SELECT 
                    w.Id, w.HomeId, w.Name, w.Description, w.IconCode, w.PointsCost, 
                    w.CreatedDateUtc, w.UpdatedDateUtc, w.FullFilledDateUtc,
                    c.Id, c.HomeId, c.Name, c.DOB, c.Gender, c.IconCode, c.PointsEarned,
                    p.Id, p.HomeId, p.Name, p.DOB, p.Role, p.IconCode
                FROM {WishesTable} w
                LEFT JOIN {ChildrenTable} c ON w.WisherId = c.Id
                LEFT JOIN {ParentTable} p ON w.GenieId = p.Id
                WHERE w.WisherId = @WisherId
            ";
        
        var wishEntities =
            await con.QueryAsync(query, _mapEntitiesToWishModel,
                new { WisherId = childId });
        return wishEntities.ToList();
    }
    
    private readonly Func<WishEntity, ChildEntity, ParentEntity, Wish> _mapEntitiesToWishModel = (w, c, p) =>
    {
        var wish = w.ToWish();
        wish.Wisher = c.ToChild();
        wish.Genie = p.ToParent();
        return wish;
    };



    public async Task<Guid> InsertWish(CreateWishRequest request)
    {
        var wishEntity = request.ToWishEntity();
        using var con = connectionFactory.GetFamilyMerchandiseDBConnection();
        var query =
            $"INSERT INTO {WishesTable} (Name, HomeId, IconCode, Description, GenieId, WisherId) VALUES (@Name, @HomeId, @IconCode, @Description, @GenieId, @WisherId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, wishEntity);
    }
}