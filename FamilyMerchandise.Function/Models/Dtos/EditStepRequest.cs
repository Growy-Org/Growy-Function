namespace FamilyMerchandise.Function.Models.Dtos;

public record EditStepRequest
{
    public Guid StepId { get; init; }
    public string StepDescription { get; init; } = string.Empty;
}