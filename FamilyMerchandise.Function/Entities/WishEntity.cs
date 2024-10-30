namespace FamilyMerchandise.Function.Entities;

public record WishEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int IconCode { get; set; }
    public int? PointsCost { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? FullFilledDateUtc { get; set; }
}