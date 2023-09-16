using Application.Services;

namespace Infrastructure;

public class FakeSmsService : ISmsService
{
    public async Task<bool> SendAsync(string mobileNumber, CancellationToken contextCancellationToken)
    {
        return true;
    }
}