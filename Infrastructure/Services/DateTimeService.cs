using Application.Repository;
using Application.Services;

namespace Infrastructure;

public class DateTimeService : IDateTimeService
{
    public DateTime Now { get; } = DateTime.Now;
    public DateTime Utc { get; } = DateTime.UtcNow;
}