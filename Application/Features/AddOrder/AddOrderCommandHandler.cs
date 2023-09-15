using Application.Dto;
using Application.Events;
using Application.Repository;
using Application.Services;
using Domain.Entity;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Features.AddOrder;

public class AddOrderCommandHandler : MediatR.IRequestHandler<AddOrderCommand, OrderResultDto>
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<AddOrderCommandHandler> _logger;
    private readonly IBus _bus;

    public AddOrderCommandHandler(
        IUserService userService,
        IDateTimeService dateTimeService,
        IUnitOfWork unitOfWork,
        ILogger<AddOrderCommandHandler> logger,
        IBus bus)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _bus = bus;
        _dateTimeService = dateTimeService;
    }

    public async Task<OrderResultDto> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUser = await _userService.GetCurrentUserAsync(cancellationToken);
            
            var order = new Order(
                userId: currentUser.Id,
                productId: request.ProductId,
                partId: request.PartId,
                createdDateTime: _dateTimeService.Now);
            await _unitOfWork.OrderRepository.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation(message:"Order Created");
            
            var orderAddedEvent = new OrderCreated(
                OrderId: order.Id,
                ProductId: request.ProductId,
                UserId: currentUser.Id,
                MobileNumber: currentUser.MobileNumber,
                Email: currentUser.Email);
            await _bus.Publish(orderAddedEvent, cancellationToken);
            _logger.LogInformation(message:"Order Created Event Raised");

            return new OrderResultDto(order.Id,order.CreatedDateTime);
        }
        catch (Exception exception)
        {
            _logger.LogCritical(message: exception.Message);
            return new OrderResultDto(0,null);
        }
    }
}