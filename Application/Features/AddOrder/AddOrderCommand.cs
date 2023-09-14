using Domain.Entity;
using MediatR;

namespace Application.Features.AddOrder;



public class AddOrderCommand : IRequest<bool>
{
    public int ProductId { get; set; }
    public int PartId { get; set; }
}

