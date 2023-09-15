using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebHost.Extensions;

public static class JwtExtension
{
    public static void Jwt(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]!);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        });
    }
}