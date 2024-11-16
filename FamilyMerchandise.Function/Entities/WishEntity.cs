namespace FamilyMerchandise.Function.Entities;

public record WishEntity
{
    public Guid Id { get; init; }
    public Guid HomeId { get; init; }
    public Guid GenieId { get; init; }
    public Guid WisherId { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int IconCode { get; set; }
    public int PointsCost { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? FullFilledDateUtc { get; set; }
}