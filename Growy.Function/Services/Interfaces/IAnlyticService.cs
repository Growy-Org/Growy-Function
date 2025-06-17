using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IAnalyticService
{
    public Task<AnalyticProfile> GetAllParentsToAllChildAnalyticLive(Guid homeId, int? year);
    public Task<AnalyticProfile> GetAllParentsToOneChildAnalyticLive(Guid homeId, int? year, Guid childId);
}