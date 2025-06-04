namespace Growy.Function.Models.Dtos;

public record StepRequest
{
    // If steps are removed in between, this order may not be sequential, but that's ok as long as it's sorted, later added step always appear last
    public int StepOrder { get; init; }

    public string Description { get; init; } = string.Empty;
}