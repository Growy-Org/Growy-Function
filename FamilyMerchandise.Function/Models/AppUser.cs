namespace FamilyMerchandise.Function.Models;

public record AppUser
{
    public Guid? Id { get; set; }
    public string Email { get; init; } = string.Empty;
    public string? IdentityProvider { get; set; } = string.Empty;
    public string IdpId { get; init; } = string.Empty;
}