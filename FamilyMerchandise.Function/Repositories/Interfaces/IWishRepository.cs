using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IWishRepository
{
    public Task<List<Wish>> GetAllWishesByHomeId(Guid homeId);
    public Task<List<Wish>> GetAllWishesByChildId(Guid childId);
    public Task<Guid> InsertWish(CreateWishRequest request);
    public Task<Guid> EditWishByWishId(EditWishRequest request);
}