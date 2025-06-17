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
    public async Task<AnalyticProfile> GetAllParentsToAllChildAnalyticLive(Guid homeId, int? year)
    {
        logger.LogInformation($"Getting all parent to all child analytic for homeId {homeId}");
        return await analyticRepository.GetAnalyticLive(homeId, year ?? DateTime.Now.Year);
    }

    public async Task<AnalyticProfile> GetAllParentsToOneChildAnalyticLive(Guid homeId, int? year, Guid childId)
    {
        logger.LogInformation($"Getting all parent to one child analytic for homeId {homeId} and childId {childId}");
        return await analyticRepository.GetAnalyticLive(homeId, year ?? DateTime.Now.Year, childId);
    }
}