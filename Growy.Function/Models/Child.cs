using System.Text.Json.Serialization;

namespace Growy.Function.Models;

public record Child
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ChildGender Gender { get; set; }
    public DateTime DOB { get; set; }
    public int? PointsEarned { get; set; }
}

public enum ChildGender
{
    GIRL,
    BOY
}