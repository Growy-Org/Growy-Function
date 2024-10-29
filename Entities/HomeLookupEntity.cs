
public interface HomeLookupEntity
{
    public Guid Id { get; set; }
    public Guid HomeId { get; set; }
    public Guid ParentId { get; set; }
    public Guid ChildId { get; set; }
}
