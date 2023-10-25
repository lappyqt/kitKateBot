using Newtonsoft.Json;

namespace kitKateBot.Models.Twitch;

public record TwitchRefreshTokenResponse
{
    [JsonProperty(PropertyName = "access_token")]
    public string AccessToken { get; init; } = string.Empty;

    [JsonProperty(PropertyName = "refresh_token")]
    public string RefreshToken { get; init; } = string.Empty;

    [JsonProperty(PropertyName = "scopes")]
    public string[] Scopes { get; init; } = new string[] {};

    [JsonProperty(PropertyName = "expires_in")]
    public string ExpiresIn { get; init; } = string.Empty;

    [JsonProperty(PropertyName = "token_type")]
    public string TokenType { get; init; } = string.Empty;
}