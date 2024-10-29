
public interface PenaltyEntity
{
    public string Name { get; set; }
    public string Reason { get; set; }
    public int IconCode { get; set; }
    public int PointsDeduced { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
}
