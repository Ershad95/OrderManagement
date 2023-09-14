using Application.Repository;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        OrderRepository = new OrderRepository(dbContext);
        UserRepository = new UserRepository(dbContext);
    }
    
    public IOrderRepository OrderRepository { get; }
    public IUserRepository UserRepository { get; }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}