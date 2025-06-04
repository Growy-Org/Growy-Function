namespace Growy.Function.Models.Dtos;

public record WishRequest
{
    public Guid ParentId { get; init; }
    public Guid ChildId { get; init; }

    public int WishCost { get; init; }
    public string WishName { get; init; } = string.Empty;
    public string WishDescription { get; init; } = string.Empty;
}