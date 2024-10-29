public interface StepEntity
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? CompletedDateUtc { get; set; }
}
