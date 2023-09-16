using Domain.Entity;

namespace Application.Services;

public interface ICurrentUserService
{
    Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken);
}