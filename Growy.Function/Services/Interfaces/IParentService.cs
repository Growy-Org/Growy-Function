using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IParentService
{
    // Read 
    public Task<Guid> GetHomeIdByParentId(Guid id);

    // Create
    public Task<Guid> AddParentToHome(Guid parentId, ParentRequest request);

    // Update
    public Task<Guid> EditParent(Guid parentId, ParentRequest request);

    // Delete
    public Task DeleteParent(Guid parentId);
}