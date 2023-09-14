using Application.Repository;
using Domain.Entity;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public UserService(IHttpContextAccessor httpContextAccessor,IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var claim = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "UserId");
        var userGuid = Guid.Parse(claim!.Value);
        return  await _userRepository.GetAsync(userGuid,cancellationToken);   
    }
}
