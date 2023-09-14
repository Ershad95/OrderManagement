using Domain.Entity;

namespace Application.Repository;

public interface IUserRepository
{
    public Task<User?> GetAsync(Guid guid,CancellationToken cancellationToken);
    public Task<User?> GetAsync(string username,string password,CancellationToken cancellationToken);
}