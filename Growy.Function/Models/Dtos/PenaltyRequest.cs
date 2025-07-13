namespace Growy.Function.Models.Dtos;

public record PenaltyRequest
{
    public Guid ParentId { get; init; }
    public Guid ChildId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int PointsDeducted { get; init; }
}