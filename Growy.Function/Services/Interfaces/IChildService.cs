using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Services.Interfaces;

public interface IChildService
{
    // Read
    public Task<Guid> GetHomeIdByChildId(Guid id);

    // Create
    public Task<Guid> AddChildToHome(Guid homeId, ChildRequest child);

    // Update
    public Task<Guid> EditChild(Guid childId, ChildRequest request);

    // Delete
    public Task DeleteChild(Guid childId);
}