using Application.Services;

namespace Infrastructure;

public class FakeEmailService : IEmailService
{
    public Task<bool> SendAsync(string email, CancellationToken contextCancellationToken)
    {
        return Task.FromResult(true);
    }
}