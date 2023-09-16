using Application.Dto;
using Application.Services;
using MediatR;

namespace Application.Features.User.SignIn;

public class SignInCommandHandler : IRequestHandler<SignInCommand,TokenDto>
{
    private readonly IJwtManager _jwtManager;

    public SignInCommandHandler(IJwtManager jwtManager)
    {
        _jwtManager = jwtManager;
    }
    public async Task<TokenDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        return await _jwtManager.GetTokenAsync(request.UserName, request.Password,cancellationToken);
    }
}