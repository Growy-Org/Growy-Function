using Growy.Function.Models;

namespace Growy.Function.Entities;

public record HomeEntity
{
    public Guid Id { get; init; }
    public Guid AppUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedDateUtc { get; init; }
}
