using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IPenaltyRepository
{
    public Task<Guid> GetHomeIdByPenaltyId(Guid penaltyId);
    public Task<List<Penalty>> GetAllPenaltiesByHomeId(Guid homeId, int pageNumber, int pageSize);
    public Task<Penalty> GetPenaltyById(Guid penaltyId);
    public Task<List<Penalty>> GetAllPenaltiesByParentId(Guid parentId, int pageNumber, int pageSize);
    public Task<List<Penalty>> GetAllPenaltiesByChildId(Guid childId, int pageNumber, int pageSize);
    public Task<CreatePenaltyEntityResponse> InsertPenalty(Guid homeId, PenaltyRequest request);
    public Task<EditPenaltyEntityResponse> EditPenaltyByPenaltyId(Guid penaltyId, PenaltyRequest request);
    public Task<DeletePenaltyEntityResponse> DeletePenaltyByPenaltyId(Guid penaltyId);
}