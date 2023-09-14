using Application.Features.AddOrder;
using Application.Features.DeleteOrder;
using Application.Features.UpdateOrder;
using AutoMapper;
using WebHost.ViewModels;

namespace WebHost;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AddOrderVm, AddOrderCommand>();
        CreateMap<UpdateOrderVm, UpdateOrderCommand>();
        CreateMap<DeleteOrderVm, DeleteOrderCommand>();
    }
}