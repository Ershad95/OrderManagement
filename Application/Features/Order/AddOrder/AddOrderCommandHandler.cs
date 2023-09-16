using Application.Dto;
using Application.Events;
using Application.Repository;
using Application.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Features.Order.AddOrder;

public class AddOrderCommandHandler : MediatR.IRequestHandler<AddOrderCommand, OrderResultDto>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<AddOrderCommandHandler> _logger;
    private readonly IBus _bus;

    public AddOrderCommandHandler(
        ICurrentUserService currentUserService,
        IDateTimeService dateTimeService,
        IUnitOfWork unitOfWork,
        ILogger<AddOrderCommandHandler> logger,
        IBus bus)
    {
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _bus = bus;
        _dateTimeService = dateTimeService;
    }

    public async Task<OrderResultDto> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUser = await GetCurrentUser(cancellationToken);

            CheckValidation(request, currentUser);

            var order = new Domain.Entity.Order(
                userId: currentUser!.Id,
                productId: request.ProductId,
                partId: request.PartId,
                createdDateTime: _dateTimeService.Now);
            await _unitOfWork.OrderRepository.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation(message: "Order Created");

            var orderAddedEvent = new OrderCreated(
                OrderId: order.Id,
                ProductId: request.ProductId,
                UserId: currentUser.Id,
                MobileNumber: currentUser.MobileNumber,
                Email: currentUser.Email);
            await _bus.Publish(orderAddedEvent, cancellationToken);
            _logger.LogInformation(message: "Order Created Event Raised");

            return new OrderResultDto(order.Id, order.CreatedDateTime,"درخواست شما ثبت شد");
        }
        catch (Exception exception)
        {
            _logger.LogCritical(message: exception.Message);
            return new OrderResultDto(0, null,"مشکلی در ثبت درخواست وجود دارد");
        }
    }

    private static void CheckValidation(AddOrderCommand request, Domain.Entity.User? currentUser)
    {
        var checkPart = currentUser!.Parts!.Any(x => x.Id == request.PartId);
        if (!checkPart)
        {
            throw new InvalidOperationException($"can not add order with partId : {request.PartId}");
        }
    }

    private async Task<Domain.Entity.User?> GetCurrentUser(CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);
        if (currentUser == null)
        {
            throw new Exception("user can not found");
        }

        return currentUser;
    }
}