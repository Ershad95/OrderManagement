using Application.Dto;
using Domain.Entity;

namespace Application.Services;

public interface IJwtManager
{
    Task<TokenDto> GetTokenAsync(string username,string password,CancellationToken cancellationToken);
    Task<Guid?> GetUserIdFromToken(string token);
}