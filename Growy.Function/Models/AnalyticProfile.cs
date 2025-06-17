namespace Growy.Function.Models;

public record AnalyticProfile
{
    public int Year { get; init; }
    public int NumberOfAssignmentsAssigned { get; set; }
    public int NumberOfAssignmentsCompleted { get; set; }
    public int NumberOfAchievementsCreated { get; set; }
    public int NumberOfAchievementsGranted { get; set; }
    public int NumberOfWishesCreated { get; set; }
    public int NumberOfWishesRealised { get; set; }
    public int NumberOfPenaltyReceived { get; set; }
    public int PointsSpent { get; set; }
    public int PointsEarnedInAssignment { get; set; }
    public int PointsGrantedInAchievement { get; set; }
    public int PointsReduced { get; set; }
}