namespace FamilyMerchandise.Function.Models.Dtos;

public record EditAssignmentEntityResponse
{
    public Guid Id { get; init; }
    public Guid ChildId { get; init; }
    public int Points { get; init; }
}