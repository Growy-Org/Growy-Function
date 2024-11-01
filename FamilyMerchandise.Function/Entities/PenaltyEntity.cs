namespace FamilyMerchandise.Function.Entities;

public record PenaltyEntity
{
    public Guid Id { get; set; }
    public Guid HomeId { get; set; }
    public Guid ViolatorId { get; set; }
    public Guid EnforcerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public int IconCode { get; set; }
    public int PointsDeducted { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
}