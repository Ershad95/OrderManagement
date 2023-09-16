using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application;
using Application.Dto;
using Application.Repository;
using Application.Services;
using Domain.Entity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class JwtManager : IJwtManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _distributedCache;

    public JwtManager(
        IDateTimeService dateTimeService,
        IConfiguration configuration,
        IUnitOfWork unitOfWork,
        IDistributedCache distributedCache)
    {
        _dateTimeService = dateTimeService;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _distributedCache = distributedCache;
    }

    public async Task<TokenDto> GetTokenAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await GetUserAsync(username, password, cancellationToken);
        var key = $"user_{user!.Guid}";

        var cachedToken = await _distributedCache.GetAsync(key, cancellationToken);
        if (cachedToken != null)
        {
            return new TokenDto { Token = Encoding.UTF8.GetString(cachedToken) };
        }

        var token = await CreateTokenAsync(user, key, cancellationToken);
        return new TokenDto { Token = token };
    }


    public Task<Guid?> GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]!);
        
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var guid = Guid.Parse(jwtToken.Claims.First(x => x.Type == CustomClaim.UserGuid).Value);

            return Task.FromResult<Guid?>(guid);
        }
        catch
        {
            return Task.FromResult<Guid?>(null);
        }
    }

    private async Task<string> CreateTokenAsync(User user, string key, CancellationToken cancellationToken)
    {
        var expirationTokenDateTime = _dateTimeService.Utc.AddDays(value: 30);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(s: _configuration[key: "JWT:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims: new Claim[]
            {
                new(type: CustomClaim.UserGuid, value: user.Guid.ToString())
            }),
            Expires = expirationTokenDateTime,
            SigningCredentials = new SigningCredentials(key: new SymmetricSecurityKey(key: tokenKey),
                algorithm: SecurityAlgorithms.HmacSha256Signature)
        };
        var securityToken = tokenHandler.CreateToken(tokenDescriptor: tokenDescriptor);
        var token = tokenHandler.WriteToken(token: securityToken);

        var options = new DistributedCacheEntryOptions();
        options.SetAbsoluteExpiration(absolute: expirationTokenDateTime);

        await _distributedCache.SetAsync(key: key, value: Encoding.UTF8.GetBytes(s: token),
            options: options, token: cancellationToken);
        return token;
    }

    private async Task<User?> GetUserAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(username: username, password: password,
            cancellationToken: cancellationToken);

        if (user is null)
        {
            throw new Exception(message: "user not found");
        }

        return user;
    }
}