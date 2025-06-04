using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IParentRepository
{
    public Task<Guid> GetHomeIdByParentId(Guid parentId);
    public Task<List<Parent>> GetParentsByHomeId(Guid homeId);
    public Task<Guid> InsertParent(Guid homeId, Parent parent);
    public Task<Guid> EditParentByParentId(EditParentRequest request);
    public Task DeleteParentByParentId(Guid parentId);
}