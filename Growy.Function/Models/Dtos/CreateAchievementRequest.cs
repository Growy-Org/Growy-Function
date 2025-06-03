namespace Growy.Function.Models.Dtos;

public record CreateAchievementRequest
{
    public Guid HomeId { get; init; }
    public Guid ParentId { get; init; }
    public Guid ChildId { get; init; }
    public string AchievementName { get; init; } = string.Empty;
    public string AchievementDescription { get; init; } = string.Empty;
    public int AchievementPointsGranted { get; init; }
}