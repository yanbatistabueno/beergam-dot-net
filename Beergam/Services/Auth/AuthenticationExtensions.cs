using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Beergam.Services.Auth;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()
                          ?? throw new InvalidOperationException("A seção 'Jwt' não foi configurada.");

        if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
            throw new InvalidOperationException("Jwt:SecretKey não foi configurada.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                };
            });
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddAuthorization();
        return services;
    }
}