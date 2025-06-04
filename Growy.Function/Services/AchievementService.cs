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
    public async Task<List<Achievement>> GetAllAchievementsByParentId(Guid parentId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all achievements by Parent: {parentId}");
        var achievements = await achievementRepository.GetAllAchievementsByParentId(parentId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all achievements by Parent : {parentId}");
        return achievements;
    }

    public async Task<List<Achievement>> GetAllAchievementsByChildId(Guid childId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all achievements by ChildId: {childId}");
        var achievements = await achievementRepository.GetAllAchievementsByChildId(childId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all achievements by ChildId : {childId}");
        return achievements;
    }

    public async Task<List<Achievement>> GetAllAchievementsByHomeId(Guid homeId, int pageNumber, int pageSize)
    {
        logger.LogInformation($"Getting all achievements by HomeId: {homeId}");
        var achievements = await achievementRepository.GetAllAchievementsByHomeId(homeId, pageNumber, pageSize);
        logger.LogInformation(
            $"Successfully getting all achievements by HomeId : {homeId}");
        return achievements;
    }

    public async Task<Achievement> GetAchievementById(Guid achievementId)
    {
        logger.LogInformation($"Getting achievement by Id: {achievementId}");
        var achievement = await achievementRepository.GetAchievementById(achievementId);

        logger.LogInformation(
            $"Successfully getting achievement by Id: {achievement.Id}");
        return achievement;
    }

    // Create
    public async Task<Guid> CreateAchievement(CreateAchievementRequest request)
    {
        logger.LogInformation($"Adding a new Achievement to Home: {request.HomeId}");
        var assignmentId = await achievementRepository.InsertAchievement(request);
        logger.LogInformation(
            $"Successfully added an Achievement : {assignmentId}, by Parent {request.ParentId} to Child {request.ChildId}");
        return assignmentId;
    }

    // Update
    public async Task<Guid> EditAchievement(EditAchievementRequest request)
    {
        logger.LogInformation($"Editing achievement {request.AchievementId}");
        var id = await achievementRepository.EditAchievementByAchievementId(request);
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