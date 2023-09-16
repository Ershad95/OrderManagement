using Application.Services;

namespace Infrastructure.Services;

public class FakeSmsService : ISmsService
{
    public async Task<bool> SendAsync(string mobileNumber, CancellationToken contextCancellationToken)
    {
        return true;
    }
}