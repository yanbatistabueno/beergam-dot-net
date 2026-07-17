using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                };
                options.MapInboundClaims = false;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["access_token"];
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var authCacheService = context.HttpContext.RequestServices.GetRequiredService<IAuthCache>();
                        var pin = context.Principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                        var jti   = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                        if (pin == null || jti == null)
                        {
                            context.Fail("Token inválido.");
                        }
                        var currentJti = authCacheService.GetCacheUserCurrentJti(pin).Result;
                        if (currentJti == null || currentJti != jti)
                        {
                            context.Fail("Token expirado ou revogado.");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
        return services;
    }
}