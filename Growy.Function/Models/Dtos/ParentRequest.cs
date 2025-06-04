using System.Text.Json.Serialization;

namespace Growy.Function.Models.Dtos;

public record ParentRequest
{
    public string Name { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ParentRole Role { get; set; }

    public DateTime DOB { get; set; }
}