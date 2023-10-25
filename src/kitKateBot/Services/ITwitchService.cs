using kitKateBot.Models.Twitch;

namespace kitKateBot.Services;

public interface ITwitchService 
{   
    Task<TwitchUserResponse> GetUser(string login);
    Task<TwitchAuthorizationTokenResponse> RequestAccessToken(string code);
    Task<TwitchRefreshTokenResponse> RefreshAcсessToken(string accessToken);
}