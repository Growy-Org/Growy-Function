using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public interface IChildRepository
{
    public Task<Guid> InsertChild(Guid homeId, Child child);
}