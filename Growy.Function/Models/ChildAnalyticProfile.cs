using System.Text.Json.Serialization;

namespace Growy.Function.Models;

public record ChildAnalyticProfile
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ChildAnalyticViewType ViewType { get; set; }
    public int Year { get; init; }
    public int AssignmentsCompleted { get; set; }
    public int AchievementsAchieved { get; set; }
    public int PenaltyReceived { get; set; }
    public int WishesRealised { get; set; }
    public int PointsSpent { get; set; }
    public int PointsGained { get; set; }
    public int PointsReduced { get; set; }
}

public enum ChildAnalyticViewType
{
    ByChild,
}