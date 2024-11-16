using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IChildRepository
{
    public Task<List<Child>> GetChildrenByHomeId(Guid homeId);
    public Task<Guid> InsertChild(Guid homeId, Child child);
    public Task<Guid> EditPointsByChildId(Guid childId, int deltaPoints);
}