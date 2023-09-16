using MediatR;

namespace Application.Features.Order.DeleteOrder;

public class DeleteOrderCommand : IRequest<DeleteOrderDto>
{
    public int Id { get; set; }
}

public record DeleteOrderDto(bool IsSuccess);
