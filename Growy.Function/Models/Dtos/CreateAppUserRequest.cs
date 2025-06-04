namespace Growy.Function.Models.Dtos;

public record CreateAppUserRequest
{
    public string Email { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
}