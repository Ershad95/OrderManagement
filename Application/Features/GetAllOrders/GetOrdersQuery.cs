using Application.Dto;
using Domain.Entity;
using MediatR;

namespace Application.Features.GetAllOrders;

public class GetOrdersQuery : IRequest<IEnumerable<OrderDto>>
{
    public bool ShowAllRequest { get; set; }
}