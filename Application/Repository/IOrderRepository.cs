using Application.Dto;
using Domain.Entity;

namespace Application.Repository;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync(int userId, CancellationToken cancellationToken);
    Task<Order?> GetOrderAsync(int id, int userId, CancellationToken cancellationToken);
}