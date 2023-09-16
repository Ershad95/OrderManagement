namespace Application.Repository;

public interface IUnitOfWork
{
    public IOrderRepository OrderRepository { get; }
    
    public IUserRepository UserRepository { get; }

    public Task SaveAsync(CancellationToken cancellationToken);
}