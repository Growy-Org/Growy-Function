namespace FamilyMerchandise.Function.Models;

public record Penalty
{
    public string Name { get; set; }
    public string Reason { get; set; }
    public int IconCode { get; set; }
    public int PointsDeduced { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public Child Violator { get; set; }
    public Parent Enforcer { get; set; }
}