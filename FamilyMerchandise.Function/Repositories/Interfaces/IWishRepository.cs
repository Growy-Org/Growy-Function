using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IWishRepository
{
    public Task<List<Wish>> GetAllWishesByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<Wish> GetWishById(Guid wishId);
    public Task<List<Wish>> GetAllWishesByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<List<Wish>> GetAllWishesByChildId(Guid childId, int pageNumber, int pageSize);
    public Task<Guid> InsertWish(CreateWishRequest request);
    public Task<Guid> EditWishByWishId(EditWishRequest request);
    public Task<EditWishEntityResponse> EditWishFulFillStatusByWishId(Guid wishId, bool isFulFilled);
    public Task DeleteWishByWishId(Guid wishId);
}