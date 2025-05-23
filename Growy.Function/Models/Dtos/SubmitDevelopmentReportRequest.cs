namespace Growy.Function.Models.Dtos;

public record SubmitDevelopmentReportRequest
{
    public Guid HomeId { get; init; }
    public Guid ExaminerId { get; init; }
    public Guid CandidateId { get; init; }
    public string DevelopmentQuotientTestAnswer { get; init; } = string.Empty;
}