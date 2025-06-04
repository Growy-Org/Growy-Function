using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IParentRepository
{
    public Task<Guid> GetHomeIdByParentId(Guid parentId);
    public Task<List<Parent>> GetParentsByHomeId(Guid homeId);
    public Task<Guid> InsertParent(Guid homeId, ParentRequest request);
    public Task<Guid> EditParentByParentId(Guid parentId, ParentRequest request);
    public Task DeleteParentByParentId(Guid parentId);
}