namespace FamilyMerchandise.Function.Models;

public record Home
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public List<Parent> Parents { get; set; }
    public List<Child> Children { get; set; }
}