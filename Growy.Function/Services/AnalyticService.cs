using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;
using Growy.Function.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class AnalyticService(
    IAnalyticRepository analyticRepository,
    ILogger<AnalyticService> logger)
    : IAnalyticService
{
    public async Task<ParentAnalyticProfile> GetAllParentsToAllChildAnalyticLive(Guid homeId, int? year)
    {
        return await analyticRepository.GetAllParentsToAllChildAnalytic(homeId, year ?? DateTime.Now.Year);
    }
    
    public async Task<ParentAnalyticProfile> GetAllParentsToOneChildAnalyticLive(Guid childId, int? year)
    {
        return await analyticRepository.GetAllParentsToOneChildAnalytic(childId, year ?? DateTime.Now.Year);
    }
    
    public async Task<ChildAnalyticProfile> GetChildAnalyticByChildIdLive(Guid childId, int? year)
    {
        return await analyticRepository.GetChildAnalyticByChildId(childId, year ?? DateTime.Now.Year);
    }
}