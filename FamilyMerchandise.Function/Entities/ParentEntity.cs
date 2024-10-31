namespace FamilyMerchandise.Function.Entities;

public record ParentEntity
{
    public Guid Id { get; set; }
    public Guid HomeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int IconCode { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime DOB { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
}