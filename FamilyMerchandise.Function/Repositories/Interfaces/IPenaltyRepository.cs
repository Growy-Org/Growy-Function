using FamilyMerchandise.Function.Models;
using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IPenaltyRepository
{
    public Task<List<Penalty>> GetAllPenaltiesByHomeId(Guid homeId);
    public Task<List<Penalty>> GetAllPenaltiesByChildId(Guid childId);
    public Task<Guid> InsertPenalty(CreatePenaltyRequest request);
    public Task<EditPenaltyEntityResponse> EditPenaltyByPenaltyId(EditPenaltyRequest request);
}