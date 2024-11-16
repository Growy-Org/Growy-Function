
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IHomeRepository
{
    public Task<Home> GetHome(Guid homeId);
    public Task<Guid> InsertHome(Home home);
    public Task<Guid> EditHomeByHomeId(EditHomeRequest request);

}