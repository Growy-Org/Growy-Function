
namespace FamilyMerchandise.Function.Repositories;

public interface IChildRepository
{
    public Task<int> GetChildById();
    public Task<int> GetChildrenByParentId();
}