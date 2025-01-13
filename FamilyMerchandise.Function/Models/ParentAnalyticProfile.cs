using System.Text.Json.Serialization;

namespace FamilyMerchandise.Function.Models;

public record ParentAnalyticProfile
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ParentAnalyticViewType ViewType { get; set; }

    public int Year { get; init; }
    public int AssignmentsAssigned { get; set; }
    public int AchievementsGranted { get; set; }
    public int WishesFulfilled { get; set; }

    public int PenaltiesSignedOff { get; set; }
}

public enum ParentAnalyticViewType
{
    AllParentsToAllChildren,
    AllParentsToOneChild,
    OneParentToOneChild,
}