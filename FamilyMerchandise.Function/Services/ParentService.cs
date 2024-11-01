using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories;
using Microsoft.Extensions.Logging;

namespace FamilyMerchandise.Function.Services;

public class ParentService(
    IAssignmentRepository assignmentRepository,
    IAchievementRepository achievementRepository,
    ILogger<ParentService> logger)
    : IParentService
{
    public List<Assignment> GetAllAssignmentsByHomeId(Guid homeId)
    {
        throw new NotImplementedException();
    }

    public Assignment GetAssignment(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> CreateAssignment(CreateAssignmentRequest request)
    {
        logger.LogInformation($"Adding a new Assignment to Home: {request.HomeId}");
        var assignmentId = await assignmentRepository.InsertAssignment(request);
        logger.LogInformation(
            $"Successfully added an Assignment : {assignmentId}, by Parent {request.ParentId} to Child {request.ChildId}");
        return assignmentId;
    }

    public void EditAssignment(Guid assignmentId, Assignment assignment)
    {
        throw new NotImplementedException();
    }

    public void CompleteAssignment(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public Assignment CreateStepToAssignment(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public void EditStep(Guid stepId)
    {
        throw new NotImplementedException();
    }

    public List<Wish> GetWishesByHomeId(Guid homeId)
    {
        throw new NotImplementedException();
    }

    public Wish EditWishCost(Guid wishId)
    {
        throw new NotImplementedException();
    }

    public List<Assignment> GetAllAchievementByHomeId(Guid homeId)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> CreateAchievement(CreateAchievementRequest request)
    {
        logger.LogInformation($"Adding a new Achievement to Home: {request.HomeId}");
        var assignmentId = await achievementRepository.InsertAchievement(request);
        logger.LogInformation(
            $"Successfully added an Achievement : {assignmentId}, by Parent {request.ParentId} to Child {request.ChildId}");
        return assignmentId;
    }

    public void EditAchievement(Guid achievementId)
    {
        throw new NotImplementedException();
    }

    public void GrantAchievementBonus(Guid achievementId)
    {
        throw new NotImplementedException();
    }

    public List<Penalty> GetAllPenaltiesByHomeId(Guid homeId)
    {
        throw new NotImplementedException();
    }

    public Penalty CreatePenalty()
    {
        throw new NotImplementedException();
    }
}