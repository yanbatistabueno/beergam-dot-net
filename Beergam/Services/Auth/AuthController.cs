using Microsoft.AspNetCore.Mvc;

namespace Beergam.Services.Auth;

public class AuthController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}