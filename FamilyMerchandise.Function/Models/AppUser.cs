namespace FamilyMerchandise.Function.Models;

public record AppUser
{
    public Guid Id { get; init; }
    public string Email { get; set; } = string.Empty;
    public string? IdentityProvider { get; set; } = string.Empty;
    public string? IdpId { get; set; } = string.Empty;
}