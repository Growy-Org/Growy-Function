namespace Growy.Function.Entities.EntityResponse;

public record EditAchievementEntityResponse
{
    public Guid Id { get; init; }
    public Guid ChildId { get; init; }
    public int Points { get; init; }
}