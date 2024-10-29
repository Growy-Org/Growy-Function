
public interface ChildEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int IconCode { get; set; }
    public DateTime DOB { get; set; }
    public int PointsEarned { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
}
