namespace Growy.Function.Models;

public record Penalty
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public int IconCode { get; set; }
    public int PointsDeducted { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public Child Violator { get; set; } = new();
    public Parent Enforcer { get; set; } = new();
}