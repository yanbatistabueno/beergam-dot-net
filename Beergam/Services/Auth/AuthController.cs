using Beergam.Api;
using Beergam.Services.Password;
using Beergam.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Beergam.Services.Auth;

public class AuthController : ApiController
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [AllowAnonymous]
    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] AuthDTO.LoginRequestDto request)
    {
        try
        {
            var response = await _authService.Login(request);
            SetTokenCokkie(response.Token);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] AuthDTO.RegisterRequestDto request)
    {
        try
        {
            var response = await _authService.Register(request);
            SetTokenCokkie(response.Token);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }

    private void SetTokenCokkie(string token)
    {
        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            
        });
    }
}