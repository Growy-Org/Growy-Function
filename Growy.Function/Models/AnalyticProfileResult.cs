namespace Growy.Function.Models;

public record AnalyticProfileResult<T>()
{
    public RequestStatus Status { get; set; }
    public T? Result { get; set; }
    public string Message { get; set; } = String.Empty;
}

public enum RequestStatus
{
    Success,
    Failure,
}