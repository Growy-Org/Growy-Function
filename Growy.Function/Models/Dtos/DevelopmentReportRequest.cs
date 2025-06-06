namespace Growy.Function.Models.Dtos;

public record DevelopmentReportRequest
{
    public Guid ParentId { get; init; }
    public Guid ChildId { get; init; }
    public List<int> Answers { get; init; } = new();
    public int TotalScore { get; init; }
    public float DqResult { get; init; }
    public float CandidateMonth { get; init; }
}