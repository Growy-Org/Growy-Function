using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Repositories;
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


    public Child GetProfileByChildId(Guid childId)
    {
        throw new NotImplementedException();
    }

    public List<Assignment> GetAllAssignmentsByChildId(Guid childId)
    {
        throw new NotImplementedException();
    }

    public Assignment GetAssignment(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public List<Wish> GetWishes()
    {
        throw new NotImplementedException();
    }

    public Wish EditWish(Guid wishId)
    {
        throw new NotImplementedException();
    }

    public List<Achievement> GetAchievementsByChildId(Guid childId)
    {
        throw new NotImplementedException();
    }

    public List<Penalty> GetPenaltiesByChildId(Guid childId)
    {
        throw new NotImplementedException();
    }
}