using Beergam.Services.External.Marketplaces.Meli;
using Microsoft.AspNetCore.Mvc;

namespace Beergam.Services.Marketplace;

public class MarketplaceService : IMarketplaceService
{
    private readonly IMeliApiClient _meliApiClient;

    public MarketplaceService(IMeliApiClient meliApiClient)
    {
        _meliApiClient = meliApiClient;
    }

    public string GetAuthUrl()
    {
        return _meliApiClient.BuildAuthUrl();
    }
    
    public async Task<(bool success, string accessToken)> AuthenticateCallback(string code)
    {
        try
        {
            var result = await _meliApiClient.AuthenticateCallback(code);
            Console.WriteLine($"uepa mundaooooo {result.ToString()}");
            return (true, result.ToString());
        } catch (Exception e)
        {
            return (false, null);
        }
    }
}