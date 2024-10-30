namespace FamilyMerchandise.Function.Entities;

public record HomeEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDateUtc { get; set; }
}