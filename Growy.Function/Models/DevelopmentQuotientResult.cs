using Growy.Function.Models;

namespace Growy.Function.Entities;

public record DevelopmentQuotientResult
{
    public Guid Id { get; init; }
    public Child Candidate { get; set; }
    public Parent Examiner { get; set; }
    public List<int> Answers { get; init; } = new();
    public float TotalMentalAge { get; init; }
    public float DqResult { get; init; }
    public float CandidateMonth { get; init; }
    public DateTime CreatedDateUtc { get; init; }
}