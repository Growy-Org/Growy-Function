using System.Text.Json.Serialization;
using Growy.Function.Models;

namespace Growy.Function.Entities;

public record ParentEntity
{
    public Guid Id { get; set; }
    public Guid HomeId { get; set; }
    public string Name { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public DateTime DOB { get; init; }
    public DateTime CreatedDateUtc { get; init; }
    public DateTime UpdatedDateUtc { get; init; }
}