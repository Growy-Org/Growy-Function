namespace Growy.Function.Models.Dtos;

public record CreateAssignmentRequest
{
    public Guid HomeId { get; init; }
    public Guid ParentId { get; init; }
    public Guid ChildId { get; init; }
    public int AssignmentIconCode { get; init; } // initial icon
    public string AssignmentName { get; init; } = string.Empty;
    public string AssignmentDescription { get; init; } = string.Empty;
    public TimeSpan? RepeatAfter { get; init; }
    public DateTime? DueDateUtc { get; init; }
    public int Points { get; init; }
}