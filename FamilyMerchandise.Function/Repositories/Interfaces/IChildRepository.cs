using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IChildRepository
{
    public Task<List<Child>> GetChildrenByHomeId(Guid homeId);
    public Task<Guid> InsertChild(Guid homeId, Child child);
    public Task<Guid> EditPointsByChildId(Guid childId, int deltaPoints);
    public Task<Guid> EditChildByChildId(EditChildRequest request);
    public Task DeleteChildByChildId(Guid childId);
}