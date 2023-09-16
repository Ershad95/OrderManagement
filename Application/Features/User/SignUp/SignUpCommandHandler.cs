using Application.Dto;
using Application.Repository;
using Application.Services;
using AutoMapper;
using MediatR;

namespace Application.Features.User.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, TokenDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtManager _jwtManager;

    public SignUpCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IJwtManager jwtManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtManager = jwtManager;
    }

    public async Task<TokenDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var usernameIsExist =
            await _unitOfWork.UserRepository.CheckUserNameIsAvailibleAsync(request.UserName, cancellationToken);

        if (usernameIsExist)
        {
            throw new Exception("username is not valid,already selected by other user");
        }

        var user = _mapper.Map<Domain.Entity.User>(request);
        await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var token = await _jwtManager.GetTokenAsync(request.UserName, request.Password, cancellationToken);
        return token;
    }
}