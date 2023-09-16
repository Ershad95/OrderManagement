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
    private readonly IUserService _userService;
    private readonly IBus _bus;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IUnitOfWork unitOfWork,
        ILogger<DeleteOrderCommandHandler> logger,
        IUserService userService,
        IBus bus)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userService = userService;
        _bus = bus;
    }

    public async Task<DeleteOrderDto> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUser = await _userService.GetCurrentUserAsync(cancellationToken);
            
            var order = await _unitOfWork.OrderRepository.GetOrderAsync(request.Id, currentUser.Id, cancellationToken);
            if (order is null)
            {
                throw new UnauthorizedAccessException();
            }
            
            var checkPart = currentUser.Parts!.Any(x => x.Id == order.PartId);
            if (!checkPart)
            {
                throw new InvalidOperationException();
            }
            
            order.MarkAsDeleted();
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
}