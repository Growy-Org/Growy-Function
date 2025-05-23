using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IHomeRepository
{
    public Task<Home> GetHome(Guid homeId);
    public Task<Guid> InsertHome(CreateHomeRequest home);
    public Task<Guid> EditHomeByHomeId(EditHomeRequest request);
    public Task DeleteHomeByHomeId(Guid homeId);
    public Task<Guid?> GetHomeIdByAppUserId(Guid appUserId);
}