using System.Data;
using Growy.Function.Entities.EntityResponse;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IWishRepository
{
    public Task<Guid> GetHomeIdByWishId(Guid wishId);
    public Task<int> GetWishesCount(Guid homeId, Guid? parentId, Guid? childId, bool showOnlyNotFulfilled = false);

    public Task<List<Wish>> GetAllWishes(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId, bool showOnlyNotFulfilled = false);

    public Task<Guid> InsertWish(Guid homeId, WishRequest request);
    public Task<Guid> EditWishByWishId(Guid wishId, WishRequest request);

    public Task<EditWishEntityResponse> EditWishFulFillStatusByWishId(Guid wishId, bool isFulFilled, IDbConnection con,
        IDbTransaction transaction);

    public Task DeleteWishByWishId(Guid wishId);
}