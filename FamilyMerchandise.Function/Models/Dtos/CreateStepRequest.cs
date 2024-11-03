namespace FamilyMerchandise.Function.Models.Dtos;

public record CreateStepRequest
{
    public Guid AssignmentId { get; init; }

    public int StepOrder { get; init; } 
    public string StepDescription { get; init; } = string.Empty;
}