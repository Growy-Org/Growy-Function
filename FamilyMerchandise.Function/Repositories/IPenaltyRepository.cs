using FamilyMerchandise.Function.Models.Dtos;
using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Repositories;

public interface IPenaltyRepository
{
    public Task<Guid> InsertPenalty(CreatePenaltyRequest request);
}