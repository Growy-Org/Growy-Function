using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Services;

public interface IChildService
{
    // Read
    public Task<Guid> GetHomeIdByChildId(Guid id);
    // Create
    public Task<Guid> AddChildToHome(Guid childId, Child child);
    // Update
    public Task<Guid> EditChild(Child request);
    // Delete
    public Task DeleteChild(Guid childId);
}