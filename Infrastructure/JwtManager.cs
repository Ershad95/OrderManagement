using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application;
using Application.Dto;
using Application.Repository;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public class JwtManager : IJwtManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public JwtManager(IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<TokenDto> CreateTokenAsync(string username, string password,CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(username, password, cancellationToken);
        if (user == null)
        {
            throw new Exception("user not found");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(CustomClaim.UserGuid, user.Guid.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new TokenDto { Token = tokenHandler.WriteToken(token) };
    }
}