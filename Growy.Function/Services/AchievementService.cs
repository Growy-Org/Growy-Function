using Growy.Function.Models.Dtos;
using Growy.Function.Models;
using Growy.Function.Repositories.Interfaces;
using Growy.Function.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growy.Function.Services;

public class AchievementService(
    IChildRepository childRepository,
    IAchievementRepository achievementRepository,
    ILogger<AchievementService> logger)
    : IAchievementService
{
    #region Achievements

    // Read
    public async Task<int> GetAchievementsCount(Guid homeId, Guid? parentId, Guid? childId,
        bool showOnlyNotAchieved = false)
    {
        return await achievementRepository.GetAchievementsCount(homeId, parentId, childId, showOnlyNotAchieved);
    }

    public async Task<List<Achievement>> GetAllAchievements(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId,
        bool showOnlyNotAchieved = false)
    {
        logger.LogInformation("Getting all achievements");
        var achievements =
            await achievementRepository.GetAllAchievements(homeId, pageNumber, pageSize, parentId, childId,
                showOnlyNotAchieved);
        logger.LogInformation(
            $"Successfully getting all achievements by Home : {homeId}");
        return achievements;
    }

    public Task<Guid> GetHomeIdByAchievementId(Guid achievementId)
    {
        return achievementRepository.GetHomeIdByAchievementId(achievementId);
    }

    // Create
    public async Task<Guid> CreateAchievement(Guid homeId, AchievementRequest request)
    {
        logger.LogInformation($"Adding a new Achievement to Home: {homeId}");
        var assignmentId = await achievementRepository.InsertAchievement(homeId, request);
        logger.LogInformation(
            $"Successfully added an Achievement : {assignmentId}, by Parent {request.ParentId} to Child {request.ChildId}");
        return assignmentId;
    }

    // Update
    public async Task<Guid> EditAchievement(Guid achievementId, AchievementRequest request)
    {
        logger.LogInformation($"Editing achievement {achievementId}");
        var id = await achievementRepository.EditAchievementByAchievementId(achievementId, request);
        logger.LogInformation(
            $"Successfully edit achievement with id: {id}");
        return id;
    }

    public async Task<Guid> EditAchievementGrants(Guid achievementId, bool isAchievementGranted)
    {
        logger.LogInformation($"{(isAchievementGranted ? "Granting" : "Revoking")} achievement bonus");
        var response =
            await achievementRepository.EditAchievementGrantByAchievementId(achievementId, isAchievementGranted);
        logger.LogInformation(
            $"Successfully edit achievement grant with id: {response.Id}");

        var childId = await childRepository.EditPointsByChildId(response.ChildId,
            isAchievementGranted ? response.Points : -response.Points);

        logger.LogInformation(
            $"Successfully {(isAchievementGranted ? "adding" : "removing")} {response.Points} Points {(isAchievementGranted ? "to" : "from")} child profile with id: {childId}");
        return response.Id;
    }

    // Delete
    public async Task DeleteAchievement(Guid achievementId)
    {
        logger.LogInformation($"Deleting achievement {achievementId}");
        await achievementRepository.DeleteAchievementByAchievementId(achievementId);
        logger.LogInformation(
            $"Successfully deleted achievement {achievementId}");
    }

    #endregion
}