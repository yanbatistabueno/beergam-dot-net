using Beergam.Api;
using Beergam.Services.Password;
using Beergam.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Beergam.Services.Auth;

public class AuthController : ApiController
{
    private readonly IPasswordService _passwordService;
    private readonly IAuthService _authService;
    public AuthController(IPasswordService passwordService, IAuthService authService)
    {
        _passwordService = passwordService;
        _authService = authService;
    }

    [HttpPost("/login")]
    public IActionResult Login([FromBody] AuthDTO.LoginRequestDto request)
    {
        if (request.Email == "user@example.com" && _passwordService.VerifyHashedPassword("hashed_password", request.Password))
        {
            var token = "your_generated_jwt_token";
            return Ok(new { Token = token });
        }
        return Unauthorized("Credenciais inválidas.");
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] AuthDTO.RegisterRequestDto request)
    {
        try
        {
            var user = await _authService.Register(request);
            var response = new AuthDTO.RegisterResponseDto(user, "1234");
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
}