using Application.Features.AddOrder;
using Application.Features.DeleteOrder;
using Application.Features.SignUp;
using Application.Features.UpdateOrder;
using AutoMapper;
using Domain.Entity;
using WebHost.ViewModels;

namespace WebHost;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AddOrderVm, AddOrderCommand>();
        CreateMap<UpdateOrderVm, UpdateOrderCommand>();
        CreateMap<DeleteOrderVm, DeleteOrderCommand>();
        CreateMap<SignUpVm, SignUpCommand>();
        CreateMap<SignUpCommand, User>();
    }
}