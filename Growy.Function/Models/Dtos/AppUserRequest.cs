namespace Growy.Function.Models.Dtos;

public record AppUserRequest
{
    public string Email { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
}