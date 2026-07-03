using Microsoft.AspNetCore.Mvc;

namespace Beergam.Services.Auth;
using Beergam.Services.Password;
public class AuthController : Controller
{
    private readonly IPasswordService _passwordService;
    public AuthController(IPasswordService passwordService)
    {
        _passwordService = passwordService;
    }
    
    
    
}