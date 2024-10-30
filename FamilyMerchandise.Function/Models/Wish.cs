namespace FamilyMerchandise.Function.Models;

public record Wish
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int IconCode { get; set; }
    public int? PointsCost { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? FullFilledDateUtc { get; set; }
    public Child Wisher { get; set; }
    public Parent Genie { get; set; }
}