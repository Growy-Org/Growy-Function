using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public interface IParentRepository
{
    public Task<Guid> InsertParent(Guid homeId, Parent parent);
}