public interface Wish
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid IconId { get; set; }
    public int? PointsCost { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public bool IsFulfilled { get; set; }
    public Child Wisher { get; set; }
}
