using Application.Features.LogOut;
using Application.Features.SignIn;
using Application.Features.SignUp;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using WebHost.ViewModels;
using ILogger = Serilog.ILogger;

namespace WebHost.Controllers;

public class UserController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
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

    [HttpGet]
    [Route("logout")]
    public async Task<IActionResult> LogOut()
    {
        await _mediator.Send(new LogoutCommand());
        return Ok();
    }
}