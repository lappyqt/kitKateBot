using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using kitKateBot.Commands;

namespace kitKateBot.Services.Impl;

public class DiscordClientService : IDiscordClientService
{
    private readonly IConfiguration _configuration;
    private readonly DiscordClient _client;
    private readonly IServiceProvider _provider;

    public DiscordClientService(IConfiguration configuration, IServiceProvider provider)
    {
        _configuration = configuration;
        _provider = provider;

        _client = new DiscordClient(new DiscordConfiguration 
        {
            Token = _configuration["Discord:Token"],
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All,
            MinimumLogLevel = LogLevel.Debug
        });

        _client.Ready += OnClientReady;
    }

    protected async Task OnClientReady(DiscordClient client, ReadyEventArgs readyEventArgs)
    {
        var activity = new DiscordActivity 
        { 
            Name = "twitch.tv/mood_kitKate",
            ActivityType = ActivityType.Streaming,
            StreamUrl = "https://twitch.tv/mood_kitKate"
        };

        await client.UpdateStatusAsync(activity);
    }

    public async Task RunAsync()
    {   
        AddSlashCommands();

        await _client.ConnectAsync();
    }

    private void AddSlashCommands()
    {
        var commands = _client.UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = _provider
        });

        commands.RegisterCommands<TwitchCommandsModule>();
    }
}