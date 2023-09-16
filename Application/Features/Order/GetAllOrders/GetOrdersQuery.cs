using Application.Dto;
using MediatR;

namespace Application.Features.Order.GetAllOrders;

public class GetOrdersQuery : IRequest<IEnumerable<OrderDto>>
{
    public bool ShowAllRequest { get; set; }
}