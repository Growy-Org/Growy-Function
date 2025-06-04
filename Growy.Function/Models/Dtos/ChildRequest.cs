using System.Text.Json.Serialization;

namespace Growy.Function.Models.Dtos;

public record ChildRequest
{
    public string Name { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ChildGender Gender { get; set; }

    public DateTime DOB { get; set; }

    public int PointsEarned { get; init; } 
}