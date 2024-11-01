namespace FamilyMerchandise.Function.Models.Dtos;

public record CreateAchievementRequest
{
    public Guid HomeId { get; init; }
    public Guid ParentId { get; init; }
    public Guid ChildId { get; init; }
    public int AchievementIconCode { get; init; } // initial icon
    public string AchievementName { get; init; } = string.Empty;
    public string AchievementDescription { get; init; } = string.Empty;
    public int AchievementPointsGranted { get; init; }
}