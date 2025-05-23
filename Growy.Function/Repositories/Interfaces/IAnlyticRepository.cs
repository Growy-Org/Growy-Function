using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IAnalyticRepository
{
    public Task<ParentAnalyticProfile> GetAllParentsToAllChildAnalytic(Guid homeId, int year);
    public Task<ParentAnalyticProfile> GetAllParentsToOneChildAnalytic(Guid childId, int year);
    public Task<ChildAnalyticProfile> GetChildAnalyticByChildId(Guid childId, int year);
}