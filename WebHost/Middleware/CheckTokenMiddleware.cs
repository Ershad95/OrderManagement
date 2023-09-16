using System.Net;
using Application.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace WebHost.Middleware;

public class CheckTokenMiddleware : IMiddleware
{
    private readonly IDistributedCache _distributedCache;
    private readonly IJwtManager _jwtManager;

    public CheckTokenMiddleware(IDistributedCache distributedCache, IJwtManager jwtManager)
    {
        _distributedCache = distributedCache;
        _jwtManager = jwtManager;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            var auth = context.Request.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(auth))
            {
                var pureToken = auth.ToString().Split(" ")[1];
                var guid = await _jwtManager.GetUserIdFromToken(pureToken);
                if (guid is null)
                {
                    await SetResponseAsUnauthorized(context);
                    return;
                }
                
                var result = await _distributedCache.GetAsync($"user_{guid}", context.RequestAborted);
                if (result is null)
                {
                    await SetResponseAsUnauthorized(context);
                    return;
                }
            }
        }
        catch (Exception)
        {
            await SetResponseAsUnauthorized(context);
        }
        await next.Invoke(context);
    }
    
    private static async Task SetResponseAsUnauthorized(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsJsonAsync(new { message = "token is invalid" });
    }
}