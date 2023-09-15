using Application.Dto;
using MediatR;

namespace Application.Features.SignIn;

public class SignInCommand : IRequest<TokenDto>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}