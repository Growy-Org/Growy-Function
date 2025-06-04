using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IAnalyticService
{
    public Task<AnalyticProfileResult<ParentAnalyticProfile>> GetLiveParentAnalyticProfile(
        ParentAnalyticViewType viewType, string? homeId, string? parentId, string? childId, int? year);

    public Task<AnalyticProfileResult<ChildAnalyticProfile>> GetLiveChildAnalyticProfile(ChildAnalyticViewType viewType,
        string? childId, int? year);
}