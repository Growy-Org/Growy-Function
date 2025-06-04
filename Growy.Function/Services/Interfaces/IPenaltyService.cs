using Growy.Function.Models.Dtos;
using Growy.Function.Models;

namespace Growy.Function.Services.Interfaces;

public interface IPenaltyService
{
    // Read
    public Task<List<Penalty>> GetAllPenaltiesByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<List<Penalty>> GetAllPenaltiesByChildId(Guid childId, int pageNumber, int pageSize);
    public Task<List<Penalty>> GetAllPenaltiesByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<Penalty> GetPenaltyById(Guid penaltyId);
    public Task<Guid> GetHomeIdByPenaltyId(Guid penaltyId);

    // Create
    public Task<Guid> CreatePenalty(Guid homeId, PenaltyRequest request);

    // Update
    public Task<Guid> EditPenalty(Guid penaltyId, PenaltyRequest request);

    // Delete
    public Task DeletePenalty(Guid penaltyId);
}