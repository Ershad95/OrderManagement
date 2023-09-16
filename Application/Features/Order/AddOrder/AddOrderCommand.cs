using Application.Dto;
using MediatR;

namespace Application.Features.Order.AddOrder;



public class AddOrderCommand : IRequest<OrderResultDto>
{
    public int ProductId { get; set; }
    public int PartId { get; set; }
}

