namespace FamilyMerchandise.Function.Models;

public record ChildAnalyticProfile
{
    public int AssignmentsCompleted { get; set; }
    public int AchievementsAchieved { get; set; }
    public int WishesMade { get; set; }
    public int PointsGained { get; set; }
    public int PointsReduced { get; set; }
}
