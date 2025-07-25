using System.Data;
using Growy.Function.Entities.EntityResponse;
using Growy.Function.Models;
using Growy.Function.Models.Dtos;

namespace Growy.Function.Repositories.Interfaces;

public interface IPenaltyRepository
{
    public Task<Guid> GetHomeIdByPenaltyId(Guid penaltyId);
    public Task<int> GetPenaltiesCount(Guid homeId, Guid? parentId, Guid? childId);

    public Task<List<Penalty>> GetAllPenalties(Guid homeId, int pageNumber, int pageSize, Guid? parentId,
        Guid? childId);

    public Task<CreatePenaltyEntityResponse> InsertPenalty(Guid homeId, PenaltyRequest request,
        IDbConnection connection, IDbTransaction transaction);

    public Task<EditPenaltyEntityResponse> EditPenaltyByPenaltyId(Guid penaltyId, PenaltyRequest request,
        IDbConnection connection, IDbTransaction transaction);

    public Task<DeletePenaltyEntityResponse> DeletePenaltyByPenaltyId(Guid penaltyId, IDbConnection connection,
        IDbTransaction transaction);
}