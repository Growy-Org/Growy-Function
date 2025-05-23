namespace Growy.Function.Models.Dtos;

public record CreateHomeRequest
{
    public Guid AppUserId { get; init; }
    public string HomeName { get; init; } = string.Empty;
    public string HomeAddress { get; init; } = string.Empty;
}