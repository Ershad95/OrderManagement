namespace Application.Services;

public interface IDateTimeService
{
    public DateTime Now { get; }
    public DateTime Utc { get; }
}