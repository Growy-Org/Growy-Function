public interface AchievementEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int IconCode { get; set; }
    public int PointsGranted { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? AchievedDateUtc { get; set; }
}
