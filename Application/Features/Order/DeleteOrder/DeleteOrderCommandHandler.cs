using Application.Events;
using Application.Repository;
using Application.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Order.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, DeleteOrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBus _bus;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IUnitOfWork unitOfWork,
        ILogger<DeleteOrderCommandHandler> logger,
        ICurrentUserService currentUserService,
        IBus bus)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currentUserService = currentUserService;
        _bus = bus;
    }

    public async Task<DeleteOrderDto> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);
            
            var order = await CheckValidation(request, cancellationToken, currentUser);

            order!.MarkAsDeleted();
            await _unitOfWork.SaveAsync(cancellationToken);

            await _bus.Publish(new OrderDeleted(order.Id, order.ProductId, currentUser.Id), cancellationToken);

            return new DeleteOrderDto(true);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return new DeleteOrderDto(false);
        }
    }

    private async Task<Domain.Entity.Order?> CheckValidation(DeleteOrderCommand request, CancellationToken cancellationToken, 
        Domain.Entity.User? currentUser)
    {
        var order = await _unitOfWork.OrderRepository.GetOrderAsync(request.Id, currentUser!.Id, cancellationToken);
        if (order is null)
        {
            throw new UnauthorizedAccessException();
        }

        var checkPart = currentUser.Parts!.Any(x => x.Id == order.PartId);
        if (!checkPart)
        {
            throw new InvalidOperationException();
        }

        return order;
    }
}