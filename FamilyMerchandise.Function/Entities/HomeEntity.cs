using FamilyMerchandise.Function.Models;

namespace FamilyMerchandise.Function.Entities;

public record HomeEntity
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string OwnerEmail { get; set; } = string.Empty;
    
    public DateTime CreatedDateUtc { get; init; }
}
