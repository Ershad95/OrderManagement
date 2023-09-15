using Application.Features.SignIn;
using Application.Features.SignUp;
using Application.Services;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebHost.ViewModels;

namespace WebHost;

public class UserController : Controller
{
    private readonly IJwtManager _jwtManager;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IJwtManager jwtManager, IMediator mediator, IMapper mapper)
    {
        _jwtManager = jwtManager;
        _mediator = mediator;
        _mapper = mapper;
    }

   
    [AllowAnonymous]
    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInVm signInVm, CancellationToken cancellationToken)
    {
        var signInCommand = _mapper.Map<SignInCommand>(signInVm);
        var response = await _mediator.Send(signInCommand, cancellationToken);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpVm signUpVm, CancellationToken cancellationToken)
    {
        var addUserCommand = _mapper.Map<SignUpCommand>(signUpVm);
        var response = await _mediator.Send(addUserCommand, cancellationToken);
        return Ok(response);
    }
}