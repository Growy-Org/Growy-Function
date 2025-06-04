namespace Growy.Function.Models.Dtos;

public record HomeRequest
{
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
}