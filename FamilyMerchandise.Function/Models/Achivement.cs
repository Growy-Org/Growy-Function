namespace FamilyMerchandise.Function.Models;

public record Achievement
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int IconCode { get; set; }
    public int PointsGranted { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? AchievementDateUtc { get; set; }
    public Child AchievedBy { get; set; }
    public Parent CreatedBy { get; set; }
}