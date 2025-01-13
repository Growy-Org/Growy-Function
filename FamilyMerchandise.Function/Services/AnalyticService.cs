using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FamilyMerchandise.Function.Services;

public class AnalyticService(
    IAnalyticRepository analyticRepository,
    ILogger<AnalyticService> logger)
    : IAnalyticService
{
    public async Task<AnalyticProfileResult<ParentAnalyticProfile>> GetLiveParentAnalyticProfile(
        ParentAnalyticViewType viewType, string? homeId, string? parentId, string? childId,
        int? year)
    {
        var result = new AnalyticProfileResult<ParentAnalyticProfile>
        { Status = RequestStatus.Success, Message = "Success!" };

        // TODO : validator should be on it's own 
        switch (viewType)
        {
            case ParentAnalyticViewType.AllParentsToAllChildren:
                if (!Guid.TryParse(homeId, out var homeIdGuid))
                {
                    logger.LogWarning($"Invalid homeId format: '{homeId}', for viewType {viewType.ToString()}.");
                    result.Status = RequestStatus.Failure;
                    result.Message =
                        $"Invalid homeId format: '{homeId}', Please ensure homeId is provided correctly in query param for viewType {viewType.ToString()}.";
                }
                else
                {
                    result.Result =
                        await analyticRepository.GetAllParentsToAllChildAnalytic(homeIdGuid, year ?? DateTime.Now.Year);
                }

                break;
            case ParentAnalyticViewType.AllParentsToOneChild:
                if (!Guid.TryParse(childId, out var childIdGuid))
                {
                    logger.LogWarning($"Invalid childId format: '{childId}', for viewType {viewType.ToString()}.");
                    result.Status = RequestStatus.Failure;
                    result.Message =
                        $"Invalid childId format: '{childId}', Please ensure childId is provided correctly in query param for viewType {viewType.ToString()}.";
                }
                else
                {
                    result.Result =
                        await analyticRepository.GetAllParentsToOneChildAnalytic(childIdGuid,
                            year ?? DateTime.Now.Year);
                }
                break;
            default:
                result.Status = RequestStatus.Failure;
                result.Message = $"No analytic view type '{viewType}' handler is found";
                break;
        }

        return result;
    }

    public async Task<AnalyticProfileResult<ChildAnalyticProfile>> GetLiveChildAnalyticProfile(ChildAnalyticViewType viewType, string? childId, int? year)
    {
                var result = new AnalyticProfileResult<ChildAnalyticProfile>
        { Status = RequestStatus.Success, Message = "Success!" };

        // TODO : validator should be on it's own 
        switch (viewType)
        {
            case ChildAnalyticViewType.ByChild:
                if (!Guid.TryParse(childId, out var childIdGuid))
                {
                    logger.LogWarning($"Invalid childId format: '{childId}', for viewType {viewType.ToString()}.");
                    result.Status = RequestStatus.Failure;
                    result.Message =
                        $"Invalid childId format: '{childId}', Please ensure childId is provided correctly in query param for viewType {viewType.ToString()}.";
                }
                else
                {
                    result.Result = await analyticRepository.GetChildAnalyticByChildId(childIdGuid, year ?? DateTime.Now.Year);
                }
                break;

            default:
                result.Status = RequestStatus.Failure;
                result.Message = $"No analytic view type '{viewType}' handler is found";
                break;
        }

        return result;
        
    }
}