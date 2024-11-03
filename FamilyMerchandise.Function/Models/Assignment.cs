namespace FamilyMerchandise.Function.Models;

public record Assignment
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int IconCode { get; set; }
    public List<Step> Steps { get; set; } = [];
    public int Points { get; set; }
    public TimeSpan? RepeatAfter { get; set; }
    public DateTime? DueDate { get; set; }
    public Child Assignee { get; set; } = new();
    public Parent Assigner { get; set; } = new();
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? CompletedDateUtc { get; set; }
}

public record Step
{
    public Guid Id { get; init; }
    public int StepOrder { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}