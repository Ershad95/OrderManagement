namespace Application.Services;

public interface IEmailService
{
    Task<bool> SendAsync(string email, CancellationToken contextCancellationToken);
}