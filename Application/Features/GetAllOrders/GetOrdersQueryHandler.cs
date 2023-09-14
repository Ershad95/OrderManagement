using Application.Dto;
using Application.Features.AddOrder;
using Application.Repository;
using Application.Services;
using Domain.Entity;
using MediatR;

namespace Application.Features.GetAllOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public GetOrdersQueryHandler(
        IUnitOfWork unitOfWork,
        IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await _userService.GetCurrentUserAsync(cancellationToken);
        if (currentUser == null)
        {
            throw new Exception();
        }
        
        var orders = await _unitOfWork.OrderRepository.GetAllOrdersAsync(
            request.ShowAllRequest ? 0 : currentUser.Id,
            cancellationToken);

        return orders;
    }
}