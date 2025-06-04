using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IHomeRepository
{
    public Task<Home> GetHome(Guid homeId);
    public Task<List<Home>> GetAllHomeByAppUserId(Guid appUserId);
    public Task<Guid> InsertHome(Guid appUserId, HomeRequest home);
    public Task<Guid> EditHomeByHomeId(Guid homeId, HomeRequest request);
    public Task DeleteHomeByHomeId(Guid homeId);
}