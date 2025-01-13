using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Services;

public interface IAnalyticService
{
    // Analytics
    public Task<AnalyticProfileResult<ParentAnalyticProfile>> GetLiveParentAnalyticProfile(ParentAnalyticViewType viewType, string? homeId, string? parentId, string? childId, int? year);
    public Task<AnalyticProfileResult<ChildAnalyticProfile>> GetLiveChildAnalyticProfile(ChildAnalyticViewType viewType, string? childId, int? year);

}