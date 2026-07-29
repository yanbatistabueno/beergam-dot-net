using Microsoft.AspNetCore.Mvc;

namespace Beergam.Services.Marketplace;

public class MarketplaceController : Controller
{
    private readonly IMarketplaceService _marketplaceService;

    public MarketplaceController(IMarketplaceService marketplaceService)
    {
        _marketplaceService = marketplaceService;
    }
    [HttpGet("/auth-url")]
    public IActionResult AuthUrl()
    {
        var authUrl = _marketplaceService.GetAuthUrl();
        return Ok(new { authUrl });
    }
    
    [HttpPost("/api/auth/meli/callback")]
    public async Task<IActionResult> MeliCallback([FromForm] string code)
    {
        var result = await _marketplaceService.AuthenticateCallback(code);
        return Ok(result);
    }
}