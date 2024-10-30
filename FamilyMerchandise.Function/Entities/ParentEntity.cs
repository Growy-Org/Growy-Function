namespace FamilyMerchandise.Function.Entities;

public record ParentEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int IconCode { get; set; }
    public int Role { get; set; }
    public DateTime DOB { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
}