
public interface ChildAchievementLookupEntity
{
    public Guid ChildId { get; set; }
    public Guid ParentId { get; set; }
    public Guid AchievementId { get; set; }
}
