
public interface Child
{
    public string Name { get; set; }
    public int IconCode { get; set; }
    public ChildGender Gender { get; set; }
    public DateTime DOB { get; set; }
    public int PointsEarned { get; set; }
    public List<Parent> Parents { get; set; }
}

public enum ChildGender
{
    Girl,
    Boy,
}