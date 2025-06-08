namespace Growy.Function.Models;

public record Home
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<Parent> Parents { get; set; } = new();
    public List<Child> Children { get; set; } = new();
}