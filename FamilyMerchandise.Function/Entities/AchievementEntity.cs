namespace FamilyMerchandise.Function.Entities;

public record AchievementEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid HomeId { get; set; }
    public Guid AchievedBy { get; set; }
    public Guid CreatedBy { get; set; }
    public int IconCode { get; set; }
    public int PointsGranted { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? AchievedDateUtc { get; set; }
}