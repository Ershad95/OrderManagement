using Application.Dto;
using Domain.Entity;

namespace Application.Services;

public interface IJwtManager
{
    Task<TokenDto> CreateTokenAsync(string username,string password,CancellationToken cancellationToken);
}