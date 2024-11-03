using FamilyMerchandise.Function.Models.Dtos;

namespace FamilyMerchandise.Function.Repositories.Interfaces;

public interface IPenaltyRepository
{
    public Task<Guid> InsertPenalty(CreatePenaltyRequest request);
}