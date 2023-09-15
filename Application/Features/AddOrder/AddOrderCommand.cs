using Application.Dto;
using Domain.Entity;
using MediatR;

namespace Application.Features.AddOrder;



public class AddOrderCommand : IRequest<OrderResultDto>
{
    public int ProductId { get; set; }
    public int PartId { get; set; }
}

