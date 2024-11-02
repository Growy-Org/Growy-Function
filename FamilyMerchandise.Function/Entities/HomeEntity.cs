using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Entities;

public record HomeEntity
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public DateTime CreatedDateUtc { get; set; }
}
