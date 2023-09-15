using Application.Features.AddUser;
using Application.Services;
using AutoMapper;
using Domain.Entity;
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

    /// <summary>
    /// ورود کاربر
    [AllowAnonymous]
    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInVm signInVm)
    {
        var token = await _jwtManager.CreateTokenAsync(signInVm.Username, signInVm.Password);
        return Ok(token);
    }

    /// <summary>
    /// ثبت کاربر جدید
    [AllowAnonymous]
    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpVm signUpVm, CancellationToken cancellationToken)
    {
        var addUserCommand = _mapper.Map<AddUserCommand>(signUpVm);
        var response = await _mediator.Send(addUserCommand, cancellationToken);
        return Ok(response);
    }
}

