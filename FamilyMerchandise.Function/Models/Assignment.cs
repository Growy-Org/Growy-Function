namespace FamilyMerchandise.Function.Models;

public record Assignment
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int IconCode { get; set; }
    public List<Step> Steps { get; set; }
    public int Points { get; set; }
    public TimeSpan? RepeatAfter { get; set; }
    public DateTime? DueDate { get; set; }
    public Child Assignee { get; set; }
    public Parent Assigner { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? CompletedDateUtc { get; set; }
}

public record Step
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}