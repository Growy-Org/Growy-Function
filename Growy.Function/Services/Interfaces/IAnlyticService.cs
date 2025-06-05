using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IAnalyticService
{
    public Task<ParentAnalyticProfile> GetAllParentsToAllChildAnalyticLive(Guid homeId, int? year);
    public Task<ParentAnalyticProfile> GetAllParentsToOneChildAnalyticLive(Guid childId, int? year);
    public Task<ChildAnalyticProfile> GetChildAnalyticByChildIdLive(Guid childId, int? year);
}