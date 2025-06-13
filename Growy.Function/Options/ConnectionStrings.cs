namespace Growy.Function.Options;

public class ConnectionStrings
{
    public const string KEY = "ConnectionStrings";
    // One Whole String, Only used in Dev 
    public string GrowyDB { get; set; } = string.Empty;

    public string GrowyHost { get; init; }
    public string GrowyUser { get; init; }
    public string GrowyDbName { get; init; }
}