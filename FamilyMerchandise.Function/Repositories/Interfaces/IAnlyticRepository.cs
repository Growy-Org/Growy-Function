using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IAnalyticRepository
{
    public Task<ParentAnalyticProfile> GetAllParentsToAllChildAnalytic(Guid homeId, int year);
    public Task<ParentAnalyticProfile> GetAllParentsToOneChildAnalytic(Guid childId, int year);
}