using Microsoft.AspNetCore.Mvc;

namespace Beergam.Services.External.Marketplaces.Meli;

public interface IMeliApiClient
{
    string BuildAuthUrl();
    Task<IActionResult> Authenticate();
    Task<MeliApiDTO.OauthTokenDTO?> AuthenticateCallback(string code);


}