using Application.Features.Order.AddOrder;
using Application.Features.Order.DeleteOrder;
using Application.Features.Order.GetAllOrders;
using Application.Features.Order.UpdateOrder;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebHost.ViewModels;

namespace WebHost.Controllers;

[Authorize]
[Route("/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OrderController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] AddOrderVm addOrderVm, CancellationToken cancellationToken)
    {
        var addedOrderCommand = _mapper.Map<AddOrderCommand>(addOrderVm);
        var response = await _mediator.Send(addedOrderCommand, cancellationToken);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderVm updateOrderVm,
        CancellationToken cancellationToken)
    {
        var updateOrderCommand = _mapper.Map<UpdateOrderCommand>(updateOrderVm);
        var response = await _mediator.Send(updateOrderCommand, cancellationToken);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOrder([FromBody] DeleteOrderVm deleteOrderVm,
        CancellationToken cancellationToken)
    {
        var deleteOrderCommand = _mapper.Map<DeleteOrderCommand>(deleteOrderVm);
        var response = await _mediator.Send(deleteOrderCommand, cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] bool showAll,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetOrdersQuery { ShowAllRequest = showAll },
            cancellationToken);
        return Ok(response);
    }
}