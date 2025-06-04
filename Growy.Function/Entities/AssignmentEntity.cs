namespace Growy.Function.Entities;

public record AssignmentEntity
{
    public Guid Id { get; set; }
    public Guid HomeId { get; set; }
    public Guid AssigneeId { get; init; }
    public Guid AssignerId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int Points { get; init; }
    public DateTime? DueDateUtc { get; init; }
    public DateTime CreatedDateUtc { get; init; }
    public DateTime UpdatedDateUtc { get; init; }
    public DateTime? CompletedDateUtc { get; init; }
}