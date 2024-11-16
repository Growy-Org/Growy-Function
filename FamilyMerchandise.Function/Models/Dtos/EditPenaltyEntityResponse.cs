namespace FamilyMerchandise.Function.Models.Dtos;

public record EditPenaltyEntityResponse
{
    public Guid Id { get; init; }
    public Guid OldChildId { get; init; }
    public int OldPointsDeducted { get; init; }
}