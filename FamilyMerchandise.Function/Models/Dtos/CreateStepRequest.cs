namespace FamilyMerchandise.Function.Models.Dtos;

public record CreateStepRequest
{
    public Guid AssignmentId { get; init; }
    public int StepOrder { get; init; } // If steps are removed in between, this order may not be sequential, but that's ok as long as it's sorted, later added step always appear last
    public string StepDescription { get; init; } = string.Empty;
}