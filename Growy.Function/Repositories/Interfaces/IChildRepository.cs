using Growy.Function.Entities;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IChildRepository
{
    public Task<Guid> GetHomeIdByChildId(Guid childId);
    public Task<Child> GetChildById(Guid childId);
    public Task<List<Child>> GetChildrenByHomeId(Guid homeId);
    public Task<Guid> InsertChild(Guid homeId, Child child);
    public Task<Guid> EditPointsByChildId(Guid childId, int deltaPoints);
    public Task<Guid> EditChild(Child child);
    public Task DeleteChildByChildId(Guid childId);
}