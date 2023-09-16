using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Features.LogOut;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly IDistributedCache _distributedCache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogoutCommandHandler(
        IDistributedCache distributedCache,
        IHttpContextAccessor httpContextAccessor)
    {
        _distributedCache = distributedCache;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var claim = _httpContextAccessor.HttpContext?
            .User.Claims.First(claim => claim.Type == CustomClaim.UserGuid);
        
        if (claim != null)
        {
            await _distributedCache.RemoveAsync($"user_{claim.Value}", cancellationToken);
        }
    }
}