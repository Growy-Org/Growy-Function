using Growy.Function.Models.Dtos;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;
using Growy.Function.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class WishService(
    IChildRepository childRepository,
    IWishRepository wishRepository,
    IConnectionFactory connectionFactory,
    ILogger<WishService> logger)
    : IWishService
{
    # region Wishes

    // Read
    public async Task<int> GetWishesCount(Guid homeId, Guid? parentId, Guid? childId,
        bool showOnlyNotFulfilled = false)
    {
        return await wishRepository.GetWishesCount(homeId, parentId, childId, showOnlyNotFulfilled);
    }

    public async Task<List<Wish>> GetAllWishes(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId,
        bool showOnlyNotFulfilled = false)
    {
        logger.LogInformation("Getting all achievements");
        var wishes =
            await wishRepository.GetAllWishes(homeId, pageNumber, pageSize, parentId, childId,
                showOnlyNotFulfilled);
        logger.LogInformation(
            $"Successfully getting all achievements by Home : {homeId}");
        return wishes;
    }

    public Task<Guid> GetHomeIdByWishId(Guid wishId)
    {
        return wishRepository.GetHomeIdByWishId(wishId);
    }

    // Create
    public async Task<Guid> CreateWish(Guid homeId, WishRequest wish)
    {
        logger.LogInformation($"Adding a new Wish to Home: {homeId}");
        var wishId = await wishRepository.InsertWish(homeId, wish);
        logger.LogInformation(
            $"Successfully added a wish : {wishId}");
        return wishId;
    }

    // Update
    public async Task<Guid> EditWish(Guid wishId, WishRequest request)
    {
        logger.LogInformation($"Editing wish {wishId}");
        var id = await wishRepository.EditWishByWishId(wishId, request);
        logger.LogInformation(
            $"Successfully wish edited {wishId}");
        return id;
    }

    public async Task<Guid> SetWishFulFilled(Guid wishId, bool isFulFilled)
    {
        logger.LogInformation($"{(isFulFilled ? "Full Filling" : "Un Full Filling")} wish by Parent");
        using var con = await connectionFactory.GetDBConnection();
        con.Open();
        using var transaction = con.BeginTransaction();
        var response =
            await wishRepository.EditWishFulFillStatusByWishId(wishId, isFulFilled, con, transaction);
        logger.LogInformation(
            $"Successfully edit full fill status with id: {response.Id} by Parent");

        var childId = await childRepository.EditPointsByChildId(response.ChildId,
            isFulFilled ? -response.Points : response.Points, con, transaction);

        logger.LogInformation(
            $"Successfully {(isFulFilled ? "reducing" : "adding")} {response.Points} Points {(isFulFilled ? "from" : "back")} child profile with id: {childId} by Parent");

        transaction.Commit();
        return response.Id;
    }

    // Delete
    public async Task DeleteWish(Guid wishId)
    {
        logger.LogInformation($"Deleting wish {wishId} by Parent");
        await wishRepository.DeleteWishByWishId(wishId);
        logger.LogInformation(
            $"Successfully deleted wish {wishId} by Parent");
    }

    #endregion
}