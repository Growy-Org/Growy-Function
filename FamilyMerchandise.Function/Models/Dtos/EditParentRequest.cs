using System.Text.Json.Serialization;

namespace FamilyMerchandise.Function.Models.Dtos;

public record EditParentRequest
{
    public Guid ParentId { get; init; }
    public string ParentName { get; init; } = string.Empty;
    public int ParentIconCode { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ParentRole ParentRole { get; init; }
    public DateTime ParentDoB { get; init; }
}