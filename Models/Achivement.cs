public interface Achievement
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid IconId { get; set; }
    public int PointsGranted { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public bool IsAchieved { get; set; }
    public Child Beneficiary { get; set; }
}
