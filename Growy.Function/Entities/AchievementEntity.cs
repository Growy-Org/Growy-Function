namespace Growy.Function.Entities;

public record AchievementEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid HomeId { get; set; }
    public Guid AchieverId { get; set; }
    public Guid VisionaryId { get; set; }
    public int PointsGranted { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? AchievedDateUtc { get; set; }
}