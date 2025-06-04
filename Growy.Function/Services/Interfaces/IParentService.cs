using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IParentService
{
    // Read 
    public Task<Guid> GetHomeIdByParentId(Guid id);
    
    // Create
    public Task<Guid> AddParentToHome(Guid parentId, Parent parent);
    
    // Update
    public Task<Guid> EditParent(Parent parent);
    
    // Delete
    public Task DeleteParent(Guid parentId);
}