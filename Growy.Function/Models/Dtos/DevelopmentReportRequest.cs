namespace Growy.Function.Models.Dtos;

public record DevelopmentReportRequest
{
    public Guid ParentId { get; set; }
    public Guid ChildId { get; set; }
    public List<int> Answers { get; init; } = new(); 
    public float TotalMentalAge { get; init; }
    public float DqResult { get; init; }
    public float CandidateMonth { get; init; }
}