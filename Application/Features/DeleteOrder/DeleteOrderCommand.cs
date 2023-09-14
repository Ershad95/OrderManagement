using MediatR;

namespace Application.Features.DeleteOrder;

public class DeleteOrderCommand : IRequest<DeleteOrderDto>
{
    public int Id { get; set; }
}

public record DeleteOrderDto(bool IsSuccess);
