using Application.Dto;
using Application.Repository;
using Application.Services;
using AutoMapper;
using Domain.Entity;
using MediatR;

namespace Application.Features.AddUser;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand,TokenDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtManager _jwtManager;

    public AddUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IJwtManager jwtManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtManager = jwtManager;
    }

    public async Task<TokenDto> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var usernameIsExist =
            await _unitOfWork.UserRepository.CheckUserNameIsAvailibleAsync(request.UserName, cancellationToken);

        if (usernameIsExist)
        {
            throw new Exception("username is not valid,already selected by other user");
        }

        var user = _mapper.Map<User>(request);
        await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        var token = await _jwtManager.CreateTokenAsync(request.UserName, request.Password);
        return token;
    }
}