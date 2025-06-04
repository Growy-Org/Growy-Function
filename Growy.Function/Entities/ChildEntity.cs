using System.Text.Json.Serialization;
using Growy.Function.Models;

namespace Growy.Function.Entities;

public record ChildEntity
{
    public Guid Id { get; set; }
    public Guid HomeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DOB { get; set; }
    public string Gender { get; init; } = string.Empty;
    public int PointsEarned { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
}