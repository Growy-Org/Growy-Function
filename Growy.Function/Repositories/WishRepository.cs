using Dapper;
using Growy.Function.Models.Dtos;
using Growy.Function.Entities;
using Growy.Function.Entities.EntityResponse;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;

namespace Growy.Function.Repositories;

public class WishRepository(IConnectionFactory connectionFactory) : IWishRepository
{
    public const string WishesTable = "inventory.wishes";
    public const string ChildrenTable = "inventory.children";
    public const string ParentTable = "inventory.parents";

    public async Task<Guid> GetHomeIdByWishId(Guid wishId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT HomeId FROM {WishesTable} WHERE Id = @Id
             """;
        return await con.QuerySingleAsync<Guid>(query, new { Id = wishId });
    }

    public async Task<Wish> GetWishById(Guid wishId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {WishesTable} a
                 LEFT JOIN {ChildrenTable} c ON a.WisherId = c.Id
                 LEFT JOIN {ParentTable} p ON a.GenieId = p.Id
                 WHERE a.Id = @Id
             """;
        var wishes = await con.QueryAsync(query, _mapEntitiesToWishModel, new { Id = wishId });
        return wishes.Single();
    }

    public async Task<List<Wish>> GetAllWishesByHomeId(Guid homeId, int pageNumber, int pageSize)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {WishesTable} w
                 LEFT JOIN {ChildrenTable} c ON w.WisherId = c.Id
                 LEFT JOIN {ParentTable} p ON w.GenieId = p.Id
                 WHERE w.HomeId = @HomeId
                 ORDER BY w.CreatedDateUtc ASC
                 LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;

        var wishEntities =
            await con.QueryAsync(query, _mapEntitiesToWishModel,
                new { HomeId = homeId });
        return wishEntities.ToList();
    }

    public async Task<List<Wish>> GetAllWishesByParentId(Guid parentId, int pageNumber, int pageSize)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {WishesTable} w
                 LEFT JOIN {ChildrenTable} c ON w.WisherId = c.Id
                 LEFT JOIN {ParentTable} p ON w.GenieId = p.Id
                 WHERE w.GenieId = @GenieId
                 ORDER BY w.CreatedDateUtc ASC
                 LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;

        var wishEntities =
            await con.QueryAsync(query, _mapEntitiesToWishModel,
                new { GenieId = parentId });
        return wishEntities.ToList();
    }

    public async Task<List<Wish>> GetAllWishesByChildId(Guid childId, int pageNumber, int pageSize)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                 SELECT *
                 FROM {WishesTable} w
                 LEFT JOIN {ChildrenTable} c ON w.WisherId = c.Id
                 LEFT JOIN {ParentTable} p ON w.GenieId = p.Id
                 WHERE w.WisherId = @WisherId
                 ORDER BY w.CreatedDateUtc ASC
                 LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}
             """;

        var wishEntities =
            await con.QueryAsync(query, _mapEntitiesToWishModel,
                new { WisherId = childId });
        return wishEntities.ToList();
    }

    public async Task<Guid> InsertWish(Guid homeId, WishRequest request)
    {
        var wishEntity = request.ToWishEntity();
        wishEntity.HomeId = homeId;
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"INSERT INTO {WishesTable} (Name, HomeId, Description, GenieId, WisherId) VALUES (@Name, @HomeId, @Description, @GenieId, @WisherId) RETURNING Id";
        return await con.ExecuteScalarAsync<Guid>(query, wishEntity);
    }

    public async Task<Guid> EditWishByWishId(Guid wishId, WishRequest request)
    {
        var wishEntity = request.ToWishEntity();
        wishEntity.Id = wishId;
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"""
                UPDATE {WishesTable} 
                SET Name = @Name,
                 Description = @Description,
                 PointsCost = @PointsCost,
                 GenieId = @GenieId,
                 WisherId = @WisherId
                WHERE Id = @Id
                RETURNING Id;
             """;
        return await con.ExecuteScalarAsync<Guid>(query, wishEntity);
    }

    public async Task<EditWishEntityResponse> EditWishFulFillStatusByWishId(Guid wishId,
        bool isFulFilled)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query =
            $"UPDATE {WishesTable} SET FulFilledDateUtc = @FulFilledDateUtc WHERE Id = @Id RETURNING Id, WisherId AS ChildId, PointsCost AS Points;";
        return await con.QuerySingleAsync<EditWishEntityResponse>(query,
            new { Id = wishId, FulFilledDateUtc = isFulFilled ? DateTime.UtcNow : (DateTime?)null });
    }

    public async Task DeleteWishByWishId(Guid wishId)
    {
        using var con = await connectionFactory.GetDBConnection();
        var query = $"DELETE FROM {WishesTable} where id = @Id;";
        await con.ExecuteScalarAsync<Guid>(query, new { Id = wishId });
    }

    private readonly Func<WishEntity, ChildEntity, ParentEntity, Wish> _mapEntitiesToWishModel = (w, c, p) =>
    {
        var wish = w.ToWish();
        wish.Wisher = c.ToChild();
        wish.Genie = p.ToParent();
        return wish;
    };
}