namespace Growy.Function.Models.Dtos;

public record EditAchievementEntityResponse
{
    public Guid Id { get; init; }
    public Guid ChildId { get; init; }
    public int Points { get; init; }
}