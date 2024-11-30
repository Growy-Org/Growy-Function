namespace FamilyMerchandise.Function.Models.Dtos;

public record CreateAppUserRequest
{
    public string Email { get; init; } = string.Empty;
    public string IdpId { get; init; } = string.Empty;
}