using Beergam.Api;
using Beergam.Services.Password;
using Beergam.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Beergam.Services.Auth;

public class AuthController : ApiController
{
    private readonly IAuthService _authService;
    private readonly ICookies _cookies;
    public AuthController(IAuthService authService,  ICookies cookies)
    {
        _authService = authService;
        _cookies = cookies;
    }
    [AllowAnonymous]
    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] AuthDTO.LoginRequestDto request)
    {
        try
        {
            var (user, token, refreshToken) = await _authService.Login(request);
            SetTokenCookie(token);
            SetRefreshTokenCookie(refreshToken);
            var response = new AuthDTO.LoginResponseDto(user);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] AuthDTO.RegisterRequestDto request)
    {
        try
        {
            var (user, token, refreshToken) = await _authService.Register(request);
            SetTokenCookie(token);
            SetRefreshTokenCookie(refreshToken);
            var response = new AuthDTO.RegisterResponseDto(user);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpPost("/refresh")]
    public async Task<IActionResult> RefreshTokens()
    {
        var accessToken  = Request.Cookies["access_token"];
        var refreshToken = Request.Cookies["refresh_token"];
        return Ok($"access_token={accessToken}&refresh_token={refreshToken}");
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            return Unauthorized("Se fodeu");

        try
        {
            var (token, newRefreshToken) = await _authService.RefreshToken(accessToken, refreshToken);
            SetTokenCookie(token);
            SetRefreshTokenCookie(newRefreshToken);
            return Ok();
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }
    private void SetTokenCookie(string token)
    {
        Response.Cookies.Append("access_token", token, _cookies.GetTokenCookieOptions());
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        Response.Cookies.Append("refresh_token", refreshToken, _cookies.GetRefreshTokenCookieOptions());
    }
}