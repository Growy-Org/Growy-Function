namespace FamilyMerchandise.Function.Models;

public record Wish
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int IconCode { get; set; }
    public int? PointsCost { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? FulFilledDateUtc { get; set; }
    public Child Wisher { get; set; } = new();
    public Parent Genie { get; set; } = new();
}