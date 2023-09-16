using MediatR;

namespace Application.Features.Order.UpdateOrder;

public class UpdateOrderCommand : IRequest<UpdateOrderDto>
{
    public int Id { get; set; }
    public int PartId { get; set; }
    public int ProductId { get; set; }
}

public record UpdateOrderDto(bool IsSuccess);