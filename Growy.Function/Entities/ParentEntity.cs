using System.Text.Json.Serialization;
using Growy.Function.Models;

namespace Growy.Function.Entities;

public record ParentEntity
{
    public Guid Id { get; set; }
    public Guid HomeId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int IconCode { get; init; }
    public string Role { get; init; } = string.Empty;
    public DateTime DOB { get; init; }
    public DateTime CreatedDateUtc { get; init; }
    public DateTime UpdatedDateUtc { get; init; }
}