namespace Growy.Function.Entities.EntityResponse;

public record EditPenaltyEntityResponse
{
    public Guid Id { get; init; }
    public Guid OldChildId { get; init; }
    public int OldPointsDeducted { get; init; }
}