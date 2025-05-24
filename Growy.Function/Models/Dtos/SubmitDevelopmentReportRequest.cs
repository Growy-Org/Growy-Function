namespace Growy.Function.Models.Dtos;

public record SubmitDevelopmentReportRequest
{
    public Guid HomeId { get; init; }
    public Guid ExaminerId { get; init; }
    public Guid CandidateId { get; init; }
    public List<int> Answers { get; init; } = new ();
    public int TotalScore { get; init; }
    public float DqResult { get; init; }
    public float CandidateMonth { get; init; }
}