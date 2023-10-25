using kitKateBot.Domain.Entities;
using kitKateBot.Models.Twitch;
using kitKateBot.Persistence.Repositories.Impl;
using kitKateBot.Domain.Enums;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace kitKateBot.Services.Impl;

public class TwitchService : ITwitchService
{
    protected string _providerName = "Twitch";

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IAuthorizationHistoryRepository _repository;
    private readonly IMemoryCache _cache;

    public TwitchService(HttpClient httpClient, IConfiguration configuration, IAuthorizationHistoryRepository repository, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _repository = repository;
        _cache = cache;

        _httpClient.DefaultRequestHeaders.Add("Client-Id", _configuration["Twitch:ClientId"]);
    }

    public async Task<TwitchUserResponse> GetUser(string login)
    {
        await AddAuthorizationHeader();

        var response = await _httpClient.GetAsync($"https://api.twitch.tv/helix/users?login={login}");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TwitchUserResponse?>(responseString)!;

        return result;
    }

    public async Task<TwitchAuthorizationTokenResponse> RequestAccessToken(string code)
    {
        var twitchConfiguration = _configuration.GetSection("Twitch");

        var values = new Dictionary<string, string>()
        {
            { "client_id", twitchConfiguration["ClientId"] ?? "" },
            { "client_secret", twitchConfiguration["ClientSecret"] ?? "" },
            { "code", code },
            { "grant_type", "authorization_code" },
            { "redirect_uri", twitchConfiguration["RedirectUri"] ?? "" }
        };

        var content = new FormUrlEncodedContent(values);
        var response = await _httpClient.PostAsync("https://id.twitch.tv/oauth2/token", content);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TwitchAuthorizationTokenResponse>(responseString);

        if (result is null) throw new Exception("Invalid code.");

        var authorizationHistory = new AuthorizationHistory
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken,
            TokenType = result.TokenType,
            ProviderName = _providerName,
            AuthAction = AuthAction.TokenRequest,
            ExpiresIn = long.Parse(result.ExpiresIn),
            AuthorizedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(authorizationHistory);

        return result;
    }

    public async Task<TwitchRefreshTokenResponse> RefreshAcсessToken(string refreshToken)
    {
        var twitchConfiguration = _configuration.GetSection("Twitch");

        var values = new Dictionary<string, string>()
        {
            { "client_id", twitchConfiguration["ClientId"] ?? "" },
            { "client_secret", twitchConfiguration["ClientSecret"] ?? "" },
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshToken }
        };

        var content = new FormUrlEncodedContent(values);
        var response = await _httpClient.PostAsync("https://id.twitch.tv/oauth2/token", content);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TwitchRefreshTokenResponse>(responseString);

        if (result is null) throw new Exception("Invalid refresh token");

        var authorizationHistory = new AuthorizationHistory
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken,
            TokenType = result.TokenType,
            ProviderName = _providerName,
            AuthAction = AuthAction.TokenRefresh,
            ExpiresIn = long.Parse(result.ExpiresIn),
            AuthorizedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(authorizationHistory);
        return result; 
    }

    protected async Task<string> SetAccessToken()
    {
        var lastAuth = await _repository.GetAsync(x => x.ProviderName == _providerName); 

        if (lastAuth is null) throw new Exception($"There were no authorizations with {_providerName} provider. No AccessToken was set.");

        var accessToken = lastAuth.AccessToken;
        var expiresAt = lastAuth.AuthorizedAt.AddSeconds(lastAuth.ExpiresIn);
        var expiresIn = (expiresAt - DateTime.UtcNow).TotalSeconds;

        if (DateTime.UtcNow > expiresAt) {
            var refreshReponse = await RefreshAcсessToken(lastAuth.RefreshToken);

            expiresIn = double.Parse(refreshReponse.ExpiresIn);
            accessToken = refreshReponse.AccessToken;
        }

        _cache.Set(
            "AccessToken",
            accessToken, 
            new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(expiresIn))
        );

        return accessToken;
    }

    protected async Task AddAuthorizationHeader()
    {
        _cache.TryGetValue("AccessToken", out string? accessToken);

        var isAuthHeaderSet = _httpClient.DefaultRequestHeaders.Contains("Authorization");

        if (string.IsNullOrEmpty(accessToken) && isAuthHeaderSet) {
            isAuthHeaderSet = false;
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }

        if (isAuthHeaderSet == false) {
            if (string.IsNullOrEmpty(accessToken)) {
                accessToken = await SetAccessToken();
            }

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}"); 
        }
    }
}