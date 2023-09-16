using Application.Features.Order.AddOrder;
using Application.Features.Order.DeleteOrder;
using Application.Features.Order.UpdateOrder;
using Application.Features.User.SignIn;
using Application.Features.User.SignUp;
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
        CreateMap<SignInVm, SignInCommand>();
        CreateMap<SignUpVm, SignUpCommand>();
    }
}