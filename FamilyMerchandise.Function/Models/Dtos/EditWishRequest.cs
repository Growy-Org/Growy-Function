namespace FamilyMerchandise.Function.Models.Dtos;

public record EditWishRequest
{
    public Guid WishId { get; init; }
    public Guid ParentId { get; init; }
    public Guid ChildId { get; init; }
    public int WishIconCode { get; init; }
    public string WishName { get; init; } = string.Empty;
    public string WishDescription { get; init; } = string.Empty;
    public int WishCost { get; init; }
}