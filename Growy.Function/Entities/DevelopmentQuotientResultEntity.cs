namespace Growy.Function.Entities;

public record DevelopmentQuotientResultEntity
{
    public Guid Id { get; init; }
    public Guid HomeId { get; set; }
    public Guid CandidateId { get; init; }
    public Guid ExaminerId { get; init; }
    public List<int> Answer { get; init; } = new();
    public int TotalScore { get; init; }
    public float DqResult { get; init; }
    public float CandidateMonth { get; init; }
    public DateTime CreatedDateUtc { get; init; }
    public DateTime UpdatedDateUtc { get; init; }
}