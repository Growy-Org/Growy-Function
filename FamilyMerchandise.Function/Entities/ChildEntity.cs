namespace FamilyMerchandise.Function.Entities;

public record ChildEntity
{
    public Guid Id { get; set; }
    public Guid HomeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int IconCode { get; set; }
    public DateTime DOB { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int PointsEarned { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
}