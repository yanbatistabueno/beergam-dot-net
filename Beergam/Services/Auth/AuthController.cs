using Beergam.Api;
using Beergam.Services.Password;
using Beergam.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Beergam.Services.Auth;

public class AuthController : ApiController
{
    private readonly IAuthService _authService;
    public AuthController(IPasswordService passwordService, IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] AuthDTO.LoginRequestDto request)
    {
        try
        {
            var response = await _authService.Login(request);
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
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
}