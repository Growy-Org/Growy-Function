using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IWishRepository
{
    public Task<Guid> InsertWish(CreateWishRequest request);
}