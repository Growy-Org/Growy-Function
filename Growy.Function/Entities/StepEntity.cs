namespace Growy.Function.Entities;

public record StepEntity
{
    public Guid Id { get; set; }
    public Guid AssignmentId { get; set; }
    public int StepOrder { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateTime CreatedDateUtc { get; init; }
    public DateTime UpdatedDateUtc { get; init; }
    public DateTime? CompletedDateUtc { get; init; }
}