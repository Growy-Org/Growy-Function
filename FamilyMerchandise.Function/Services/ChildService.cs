using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FamilyMerchandise.Function.Services;

public class ChildService(
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

    public Task<List<Assignment>> GetAllAssignmentsByChildId(Guid childId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Step>> GetAllStepsByAssignmentId(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public Task<Assignment> GetAssignment(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Wish>> GetAllWishesByChildId(Guid childId)
    {
        throw new NotImplementedException();
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