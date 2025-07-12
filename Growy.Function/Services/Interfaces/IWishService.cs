using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IWishService
{
    // Read
    public Task<int> GetWishesCount(Guid homeId, Guid? parentId, Guid? childId, bool showOnlyNotFulfilled = false);

    public Task<List<Wish>> GetAllWishes(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId, bool showOnlyNotFulfilled = false);

    public Task<Guid> GetHomeIdByWishId(Guid wishId);

    // Create
    public Task<Guid> CreateWish(Guid homeId, WishRequest wish);

    // Update
    public Task<Guid> EditWish(Guid wishId, WishRequest wish);
    public Task<Guid> SetWishFulFilled(Guid wishId, bool isFulFilled);

    // Delete
    public Task DeleteWish(Guid wishId);
}