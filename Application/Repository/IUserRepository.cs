using Domain.Entity;

namespace Application.Repository;

public interface IUserRepository
{
    public Task<User?> GetAsync(Guid guid,CancellationToken cancellationToken);
    public Task<User?> GetAsync(string username,string password,CancellationToken cancellationToken);
    public Task<bool> CheckUserNameIsAvailibleAsync(string username,CancellationToken cancellationToken);

    public Task AddAsync(User user,CancellationToken cancellationToken);
}