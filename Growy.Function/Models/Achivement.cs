namespace Growy.Function.Models;

public record Achievement
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int PointsGranted { get; init; }
    public DateTime CreatedDateUtc { get; init; }
    public DateTime UpdatedDateUtc { get; init; }
    public DateTime? AchievedDateUtc { get; init; }
    public Child Achiever { get; set; } = new();
    public Parent Visionary { get; set; } = new();
}