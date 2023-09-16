using Application.Services;

namespace Infrastructure.Services;

public class FakeEmailService : IEmailService
{
    public Task<bool> SendAsync(string email, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}