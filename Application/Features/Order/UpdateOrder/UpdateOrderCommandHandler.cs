using Application.Events;
using Application.Repository;
using Application.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Order.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, UpdateOrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    private readonly IBus _bus;


    public UpdateOrderCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateOrderCommandHandler> logger,
        ICurrentUserService currentUserService,
        IBus bus)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currentUserService = currentUserService;
        _bus = bus;
    }

    public async Task<UpdateOrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync(cancellationToken);

            var order = await _unitOfWork.OrderRepository.GetOrderAsync(
                id: request.Id,
                userId: currentUser!.Id,
                cancellationToken: cancellationToken);

            CheckValidation(order, currentUser);

            order!.UpdateOrder(request.ProductId, request.PartId);
            await _unitOfWork.SaveAsync(cancellationToken);
            var message = new OrderUpdated(order.Id, request.ProductId, currentUser.Id);
            
            _logger.LogInformation("Order Updated");
            await _bus.Publish(message, cancellationToken);

            return new UpdateOrderDto(true);
        }
        catch (Exception exception)
        {
            _logger.LogCritical(exception.Message);
            return new UpdateOrderDto(false);
        }
    }

    private static void CheckValidation(Domain.Entity.Order? order, Domain.Entity.User currentUser)
    {
        if (order is null)
        {
            throw new Exception("can not update this order");
        }

        var checkPart = currentUser.Parts!.Any(x => x.Id == order.PartId);
        if (!checkPart)
        {
            throw new InvalidOperationException();
        }
    }

    private async Task<Domain.Entity.User?> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);
        if (currentUser is null)
        {
            throw new Exception("user not found");
        }

        return currentUser;
    }
}