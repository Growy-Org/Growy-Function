namespace FamilyMerchandise.Function.Models.Dtos;

public record EditHomeRequest
{
    public Guid HomeId { get; init; }
    public string HomeName { get; init; } = string.Empty;
    public string HomeAddress { get; init; } = string.Empty;
}