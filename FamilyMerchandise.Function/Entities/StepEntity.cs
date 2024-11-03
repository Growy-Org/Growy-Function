namespace FamilyMerchandise.Function.Entities;

public record StepEntity
{
    public Guid Id { get; init; }
    public Guid AssignmentId { get; init; }
    public int StepOrder { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateTime CreatedDateUtc { get; init; }
    public DateTime UpdatedDateUtc { get; init; }
    public DateTime? CompletedDateUtc { get; init; }
}