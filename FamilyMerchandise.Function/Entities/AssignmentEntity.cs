namespace FamilyMerchandise.Function.Entities;

public record AssignmentEntity
{
    public Guid Id { get; init; }
    public Guid HomeId { get; set; }
    public Guid AssigneeId { get; set; }
    public Guid AssignerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int IconCode { get; set; }
    public int Points { get; set; }
    public TimeSpan? RepeatAfter { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? CompletedDateUtc { get; set; }
}