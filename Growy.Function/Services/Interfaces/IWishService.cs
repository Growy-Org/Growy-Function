using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IWishService
{
    // Read
    public Task<Guid> GetHomeIdByWishId(Guid wishId);
    public Task<List<Wish>> GetAllWishesByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<List<Wish>> GetAllWishesByChildId(Guid childId, int pageNumber, int pageSize);
    public Task<List<Wish>> GetAllWishesByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<Wish> GetWishById(Guid wishId);

    // Create
    public Task<Guid> CreateWish(Guid homeId, WishRequest wish);

    // Update
    public Task<Guid> EditWish(Guid wishId,WishRequest wish);
    public Task<Guid> SetWishFulFilled(Guid wishId, bool isFulFilled);

    // Delete
    public Task DeleteWish(Guid wishId);
}