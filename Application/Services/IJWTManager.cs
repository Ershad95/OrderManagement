using Application.Dto;

namespace Application.Services;

public interface IJwtManager
{
    Task<TokenDto> GetTokenAsync(string username,string password,CancellationToken cancellationToken);
    Task<Guid?> GetUserIdFromToken(string token);
}