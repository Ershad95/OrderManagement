using Application.Dto;
using Application.Repository;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Orders.AddAsync(order, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(int userId, CancellationToken cancellationToken)
    {
        IList<Order> orders;
        if (userId == 0)
        {
            orders = await _dbContext.Orders.Where(x =>!x.Deleted).ToListAsync(cancellationToken);
        }
        else
        {
            orders = await _dbContext.Orders.Where(x => x.UserId == userId && !x.Deleted)
                .ToListAsync(cancellationToken);
        }
        
        return orders.Select(x => new OrderDto
        {
            ProductName = x.Product.Name,
            PartName = x.Part.Name,
            UserId = x.UserId,
            PartId = x.PartId,
            ProductId = x.ProductId,
            Id = x.Id
        }).AsEnumerable();
    }

    public async Task<Order?> GetOrderAsync(int id, int userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id &&
                                                                x.UserId == userId && !x.Deleted, cancellationToken);
    }
}