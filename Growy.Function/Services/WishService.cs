using Growy.Function.Models.Dtos;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;
using Growy.Function.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class WishService(
    IChildRepository childRepository,
    IWishRepository wishRepository,
    ILogger<WishService> logger)
    : IWishService
{
    # region Wishes

    // Read

    public Task<Guid> GetHomeIdByWishId(Guid wishId)
    {
        return wishRepository.GetHomeIdByWishId(wishId);
    }

    public async Task<List<Wish>> GetAllWishesByParentId(Guid parentId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all wishes by Parent: {parentId}");
        var wishes = await wishRepository.GetAllWishesByParentId(parentId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all wishes by Parent : {parentId}");
        return wishes;
    }

    public async Task<List<Wish>> GetAllWishesByChildId(Guid childId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all wishes by ChildId: {childId}");
        var wishes = await wishRepository.GetAllWishesByChildId(childId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all wishes by ChildId : {childId}");
        return wishes;
    }

    public async Task<List<Wish>> GetAllWishesByHomeId(Guid homeId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all wishes by Home: {homeId}");
        var wishes = await wishRepository.GetAllWishesByHomeId(homeId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all wishes by Home : {homeId}");
        return wishes;
    }

    public async Task<Wish> GetWishById(Guid wishId)
    {
        logger.LogInformation($"Getting wish by Id: {wishId}");
        var wish = await wishRepository.GetWishById(wishId);

        logger.LogInformation(
            $"Successfully getting wish by Id: {wish.Id}");
        return wish;
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
        var response =
            await wishRepository.EditWishFulFillStatusByWishId(wishId, isFulFilled);
        logger.LogInformation(
            $"Successfully edit full fill status with id: {response.Id} by Parent");

        var childId = await childRepository.EditPointsByChildId(response.ChildId,
            isFulFilled ? -response.Points : response.Points);

        logger.LogInformation(
            $"Successfully {(isFulFilled ? "reducing" : "adding")} {response.Points} Points {(isFulFilled ? "from" : "back")} child profile with id: {childId} by Parent");
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