using System.Text.Json.Serialization;

namespace Growy.Function.Models;

public record AppUser
{
    public Guid? Id { get; set; }
    public string Email { get; init; } = string.Empty;
    public string? IdentityProvider { get; set; } = string.Empty;
    public string IdpId { get; init; } = string.Empty;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AppSku Sku { get; set; }
}

// Free Feature, 1 parent, 1 child, 3 Sub tasks each assignment,
public enum AppSku
{
    Free,
    Premium,
}