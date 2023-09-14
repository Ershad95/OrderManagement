using Domain.Entity;

namespace Application.Services;

public interface IUserService
{
    Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken);
}