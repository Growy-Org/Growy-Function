namespace FamilyMerchandise.Function.Entities;

public record AppUserEntity
{
    public Guid? Id { get; init; }
    public string Email { get; set; } = string.Empty;
    public string IdentityProvider { get; set; } = string.Empty;
    public string IdpId { get; set; } = string.Empty;
}