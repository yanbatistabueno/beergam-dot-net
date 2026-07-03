using Beergam.Api;
using Beergam.Services.Password;
using Beergam.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Beergam.Services.Auth;

public class AuthController : ApiController
{
    private readonly IPasswordService _passwordService;
    private readonly IUserService _userService;
    public AuthController(IPasswordService passwordService, IUserService userService)
    {
        _passwordService = passwordService;
        _userService = userService;
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
    public IActionResult Register([FromBody] AuthDTO.RegisterRequestDto request)
    {
        // var hashedPassword = _passwordService.HashPassword(request.Password);
        //
        try
        {
            //Adicionar depois lol :D
            // _userService.RegisterUserAsync(user).Wait();
            return Ok("Usuário registrado com sucesso.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
}