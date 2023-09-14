namespace Application.Services;

public interface ISmsService
{
    Task<bool> SendAsync(string mobileNumber, CancellationToken contextCancellationToken);
}
