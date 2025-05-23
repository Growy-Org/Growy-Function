using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IChildRepository
{
    public Task<Child> GetChildById(Guid childId);
    public Task<List<Child>> GetChildrenByHomeId(Guid homeId);
    public Task<Guid> InsertChild(Guid homeId, Child child);
    public Task<Guid> EditPointsByChildId(Guid childId, int deltaPoints);
    public Task<Guid> EditChildByChildId(EditChildRequest request);
    public Task DeleteChildByChildId(Guid childId);
}