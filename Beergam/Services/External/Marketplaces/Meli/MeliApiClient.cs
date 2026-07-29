using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
namespace Beergam.Services.External.Marketplaces.Meli;

public class MeliApiClient : IMeliApiClient
{
   private readonly MeliSettings _settings;
   private readonly ILogger<MeliApiClient> _logger;
   private readonly HttpClient _httpClient = new HttpClient();

   public MeliApiClient(IOptions<MeliSettings> meliSettings, ILogger<MeliApiClient> logger)
   {
      _settings = meliSettings.Value;
      _logger = logger;
   }
   public string BuildAuthUrl()
   {
      return "https://auth.mercadolivre.com.br/authorization"
             + "?response_type=code"
             + $"&client_id={_settings.ClientId}"
             + $"&redirect_uri={_settings.RedirectUri}" ;
   }

   public string BuildCallbackCodeUrl(string code)
   {
      return "https://api.mercadolibre.com/oauth/token";
   }

   public string BuildCallbackCodeBody(string code)
   {
      var body = new MeliApiDTO.OauthTokenRequestDTO
      {
         ClientId = _settings.ClientId,
         ClientSecret = _settings.ClientSecret,
         Code = code,
         RedirectUri = _settings.RedirectUri
      };
      return JsonSerializer.Serialize(body);
   }

   public async Task<IActionResult> Authenticate()
   {
      var response = await _httpClient.GetAsync(BuildAuthUrl());
      return new ContentResult
      {
         Content = await response.Content.ReadAsStringAsync(),
         ContentType = "application/json",
         StatusCode = (int)response.StatusCode
      };
   }
   
   public async Task<MeliApiDTO.OauthTokenDTO?> AuthenticateCallback(string code)
   {
      var body = new StringContent(BuildCallbackCodeBody(code), System.Text.Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync(BuildCallbackCodeUrl(code), body);
      var content = await response.Content.ReadAsStringAsync();
      try
      {
         response.EnsureSuccessStatusCode();
         var token = JsonSerializer.Deserialize<MeliApiDTO.OauthTokenDTO>(content);
         return token;
      }
      finally
      {
         _logger.LogInformation(content);
      }
   }
}