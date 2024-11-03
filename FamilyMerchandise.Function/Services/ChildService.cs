using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories;
using FamilyMerchandise.Function.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FamilyMerchandise.Function.Services;

public class ChildService(
    IAssignmentRepository assignmentRepository,
    IStepRepository stepRepository,
    IAchievementRepository achievementRepository,
    IPenaltyRepository penaltyRepository,
    IWishRepository wishRepository,
    ILogger<ParentService> logger)
    : IChildService
{
    public async Task<Guid> CreateWish(CreateWishRequest request)
    {
        logger.LogInformation($"Adding a new Assignment to Home: {request.HomeId}");
        var wishId = await wishRepository.InsertWish(request);
        logger.LogInformation(
            $"Successfully added a wish : {wishId}, by Child {request.ChildId} to Parent {request.ParentId}");
        return wishId;
    }

    public Task<Child> GetProfileByChildId(Guid childId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Assignment>> GetAllAssignmentsByChildId(Guid childId)
    {
        logger.LogInformation($"Getting all assignments by ChildId: {childId}");
        var assignments = await assignmentRepository.GetAllAssignmentsByChildId(childId);

        foreach (var assignment in assignments)
        {
            logger.LogInformation($"Getting Children Info with ChildId: {childId}");
            var steps = await stepRepository.GetAllStepsByAssignmentId(assignment.Id);
            assignment.SetSteps(steps);
        }

        logger.LogInformation(
            $"Successfully getting all assignments by ChildId : {childId}");
        return assignments;
    }

    public Task<List<Step>> GetAllStepsByAssignmentId(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public Task<Assignment> GetAssignment(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Wish>> GetAllWishesByChildId(Guid childId)
    {
        logger.LogInformation($"Getting all wishes by ChildId: {childId}");
        var wishes = await wishRepository.GetAllWishesByChildId(childId);
        logger.LogInformation(
            $"Successfully getting all wishes by ChildId : {childId}");
        return wishes;
    }

    public Task EditWish(Guid wishId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Achievement>> GetAchievementsByChildId(Guid childId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Penalty>> GetPenaltiesByChildId(Guid childId)
    {
        throw new NotImplementedException();
    }
}