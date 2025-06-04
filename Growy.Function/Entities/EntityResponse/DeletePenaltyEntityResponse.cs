namespace Growy.Function.Entities.EntityResponse;

public record DeletePenaltyEntityResponse
{
    public Guid Id { get; init; }
    public Guid ChildId { get; init; }
    public int Points { get; init; }
}