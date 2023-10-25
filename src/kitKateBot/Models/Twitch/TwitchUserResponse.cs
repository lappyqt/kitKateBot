using Newtonsoft.Json;

namespace kitKateBot.Models.Twitch;

public class TwitchUserResponse
{
    public TwitchUser[] Data { get; set; } = new TwitchUser[] {};
}