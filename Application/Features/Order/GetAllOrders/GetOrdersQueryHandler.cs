using Application.Dto;
using Application.Repository;
using Application.Services;
using MediatR;

namespace Application.Features.Order.GetAllOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetOrdersQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUserAsync(cancellationToken);

        var orders = await _unitOfWork.OrderRepository.GetAllOrdersAsync(
            request.ShowAllRequest ? 0 : currentUser!.Id,
            cancellationToken);

        return orders;
    }

    private async Task<Domain.Entity.User?> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);
        if (currentUser == null)
        {
            throw new Exception();
        }

        return currentUser;
    }
}