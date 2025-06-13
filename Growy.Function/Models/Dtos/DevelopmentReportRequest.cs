namespace Growy.Function.Models.Dtos;

public record DevelopmentReportRequest
{
    public Guid ParentId { get; set; }
    public Guid ChildId { get; set; }
    public string Answers { get; set; } = string.Empty; 
    public float TotalMentalAge { get; init; }
    public float DqResult { get; init; }
    public float CandidateMonth { get; init; }
}