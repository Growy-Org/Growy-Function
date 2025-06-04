using System.Text.Json.Serialization;

namespace Growy.Function.Models;

public record Parent
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ParentRole Role { get; set; }

    public DateTime DOB { get; set; }
}

public enum ParentRole
{
    MOTHER,
    FATHER
}