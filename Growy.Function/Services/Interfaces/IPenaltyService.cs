using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IPenaltyService
{
    // Read
    public Task<int> GetPenaltiesCount(Guid homeId, Guid? parentId, Guid? childId);

    public Task<List<Penalty>> GetAllPenalties(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId);

    public Task<Guid> GetHomeIdByPenaltyId(Guid penaltyId);

    // Create
    public Task<Guid> CreatePenalty(Guid homeId, PenaltyRequest request);

    // Update
    public Task<Guid> EditPenalty(Guid penaltyId, PenaltyRequest request);

    // Delete
    public Task DeletePenalty(Guid penaltyId);
}