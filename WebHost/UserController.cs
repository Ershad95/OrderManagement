using Application.Services;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebHost.ViewModels;

namespace WebHost;

public class UserController : Controller
{
    private readonly IJwtManager _jwtManager;
    private readonly IUserService _userService;

    public UserController(IJwtManager jwtManager, IUserService userService)
    {
        _jwtManager = jwtManager;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] UserVm userVm)
    {
        var token = await _jwtManager.CreateTokenAsync(userVm.Username,userVm.Password);
        return Ok(token);
    }

    [Authorize]
    [HttpGet]
    [Route("get")]
    public IActionResult Get(CancellationToken cancellationToken)
    {
        return Ok(_userService.GetCurrentUserAsync(cancellationToken));
    }
}