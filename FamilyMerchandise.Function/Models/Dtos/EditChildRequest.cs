using System.Text.Json.Serialization;

namespace FamilyMerchandise.Function.Models.Dtos;

public record EditChildRequest
{
    public Guid ChildId { get; init; }
    public string ChildName { get; init; } = string.Empty;
    public int ChildIconCode { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ChildGender ChildGender { get; init; }
    public DateTime ChildDOB { get; init; }
}