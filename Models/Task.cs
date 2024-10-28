public interface Task
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid IconId { get; set; }
    public List<Step> Steps { get; set; }
    public int Points { get; set; }
    public TimeSpan? RepeatAfter { get; set; }
    public TimeSpan? DueDate { get; set; }
    public Child Assignee { get; set; }
    public Child Assigner { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public bool IsCompleted { get; set; }
}

public interface Step
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}