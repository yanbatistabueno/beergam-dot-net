using Microsoft.AspNetCore.Mvc;

namespace Beergam.Services.Marketplace;

public interface IMarketplaceService
{
    public string GetAuthUrl();
    public Task<(bool success, string accessToken)> AuthenticateCallback(string code);
}