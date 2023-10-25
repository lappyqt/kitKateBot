using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace kitKateBot.Models.Twitch;

public record TwitchUser
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; init; } = string.Empty;

    [JsonProperty(PropertyName = "login")]
    public string Login { get; init; } = string.Empty;

    [JsonProperty(PropertyName = "display_name")]
    public string DisplayName { get; init; } = string.Empty;

    [JsonProperty(PropertyName = "type")]
    public string Type { get; init; } = string.Empty;

    [JsonProperty(PropertyName = "broadcaster_type")]
    public string BroadcasterType { get; init; } = string.Empty;

    [JsonProperty(PropertyName = "description")]
    public string Description { get; init; } = string.Empty;

    [JsonProperty(PropertyName = "profile_image_url")]
    public string ProfileImageUrl { get; init; } = string.Empty;

    [JsonProperty(PropertyName = "offline_image_url")]
    public string OfflineImageUrl { get; init; } = string.Empty;

    [JsonProperty(PropertyName = "view_count")]
    public int ViewCount { get; init; }

    [JsonProperty(PropertyName = "email")]
    public string Email { get; init; } = string.Empty;

    [JsonProperty(PropertyName = "created_at")]
    public string CreatedAt { get; init; } = string.Empty;
}