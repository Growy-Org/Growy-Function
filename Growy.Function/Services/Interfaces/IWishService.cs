using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IWishService
{
    // Read
    public Task<List<Wish>> GetAllWishesByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<List<Wish>> GetAllWishesByChildId(Guid childId, int pageNumber, int pageSize);
    public Task<List<Wish>> GetAllWishesByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<Wish> GetWishById(Guid wishId);

    // Create
    public Task<Guid> CreateWish(CreateWishRequest request);

    // Update
    public Task<Guid> EditWish(EditWishRequest request);
    public Task<Guid> SetWishFulFilled(Guid wishId, bool isFulFilled);

    // Delete
    public Task DeleteWish(Guid wishId);
}