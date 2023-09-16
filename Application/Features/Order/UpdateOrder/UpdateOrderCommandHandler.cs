﻿using Application.Events;
using Application.Repository;
using Application.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Order.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, UpdateOrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    private readonly IBus _bus;


    public UpdateOrderCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateOrderCommandHandler> logger,
        IUserService userService,
        IBus bus)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userService = userService;
        _bus = bus;
    }

    public async Task<UpdateOrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUser = await _userService.GetCurrentUserAsync(cancellationToken);
            var order = await _unitOfWork.OrderRepository.GetOrderAsync(
                id: request.Id,
                userId: currentUser.Id,
                cancellationToken: cancellationToken);

            if (order is null)
            {
                throw new UnauthorizedAccessException();
            }
            
            var checkPart = currentUser.Parts!.Any(x => x.Id == order.PartId);
            if (!checkPart)
            {
                throw new InvalidOperationException();
            }

            order.UpdateOrder(request.ProductId, request.PartId);
            await _unitOfWork.SaveAsync(cancellationToken);
            var message = new OrderUpdated(order.Id, request.ProductId, currentUser.Id);
            
            await _bus.Publish(message, cancellationToken);

            return new UpdateOrderDto(true);
        }
        catch (Exception exception)
        {
            _logger.LogCritical(exception.Message);
            return new UpdateOrderDto(false);
        }
    }
}
