using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IPenaltyRepository
{
    public Task<List<Penalty>> GetAllPenaltiesByHomeId(Guid homeId);
    public Task<List<Penalty>> GetAllPenaltiesByParentId(Guid parentId);
    public Task<List<Penalty>> GetAllPenaltiesByChildId(Guid childId);
    public Task<CreatePenaltyEntityResponse> InsertPenalty(CreatePenaltyRequest request);
    public Task<EditPenaltyEntityResponse> EditPenaltyByPenaltyId(EditPenaltyRequest request);
    public Task<DeletePenaltyEntityResponse> DeletePenaltyByPenaltyId(Guid penaltyId);
}