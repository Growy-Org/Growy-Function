namespace FamilyMerchandise.Function.Models.Dtos;

public record CreateHomeRequest
{
    public Guid HomeId { get; init; }
    public Guid AppUserId { get; init; } 
    public string HomeName { get; init; } = string.Empty;
    public string HomeAddress { get; init; } = string.Empty;
}