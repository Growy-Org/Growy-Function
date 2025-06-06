namespace Growy.Function.Entities;

public record WishEntity
{
    public Guid Id { get; set; }
    public Guid HomeId { get; set; }
    public Guid GenieId { get; init; }
    public Guid WisherId { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? PointsCost { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? FulFilledDateUtc { get; set; }
}