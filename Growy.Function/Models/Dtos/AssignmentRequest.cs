namespace Growy.Function.Models.Dtos;

public record AssignmentRequest
{
    public Guid ParentId { get; init; }
    public Guid ChildId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime? DueDateUtc { get; init; }
    public int Points { get; init; }
}