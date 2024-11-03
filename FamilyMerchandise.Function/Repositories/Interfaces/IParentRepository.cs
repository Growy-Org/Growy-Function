using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IParentRepository
{
    public Task<List<Parent>> GetParentsByHomeId(Guid homeId);
    public Task<Guid> InsertParent(Guid homeId, Parent parent);
}