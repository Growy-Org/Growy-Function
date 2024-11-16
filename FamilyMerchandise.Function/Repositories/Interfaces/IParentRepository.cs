using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IParentRepository
{
    public Task<List<Parent>> GetParentsByHomeId(Guid homeId);
    public Task<Guid> InsertParent(Guid homeId, Parent parent);
    public Task<Guid> EditParentByParentId(EditParentRequest request);
}