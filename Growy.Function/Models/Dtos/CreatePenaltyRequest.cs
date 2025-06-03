namespace Growy.Function.Models.Dtos;

public record CreatePenaltyRequest
{
    public Guid HomeId { get; init; }
    public Guid ParentId { get; init; }
    public Guid ChildId { get; init; }
    public string PenaltyName { get; init; } = string.Empty;
    public string PenaltyReason { get; init; } = string.Empty;
    public int PenaltyPointsDeducted { get; init; }
}