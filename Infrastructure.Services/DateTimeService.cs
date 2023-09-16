using Application.Services;

namespace Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now { get; } = DateTime.Now;
    public DateTime Utc { get; } = DateTime.UtcNow;
}