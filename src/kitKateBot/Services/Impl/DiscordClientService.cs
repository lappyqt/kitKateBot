using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using kitKateBot.Commands;

namespace kitKateBot.Services.Impl;

public class DiscordClientService : BackgroundService, IDiscordClientService
{
    private readonly IConfiguration _configuration;
    private readonly DiscordClient _client;

    public DiscordClientService(IConfiguration configuration)
    {
        _configuration = configuration;

        _client = new DiscordClient(new DiscordConfiguration 
        {
            Token = _configuration["Discord:Token"],
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All,
            MinimumLogLevel = LogLevel.Debug
        });
    }

    public async Task RunAsync()
    {   
        AddCommands();
        AddSlashCommands();

        await _client.ConnectAsync();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RunAsync();
    }

    private void AddCommands()
    {
        var commands = _client.UseCommandsNext(new CommandsNextConfiguration
        {
            StringPrefixes = new string[] { _configuration["Discord:Prefix"]! },
            EnableDms = false,
            EnableMentionPrefix = true 
        });

        commands.RegisterCommands<CommandsModule>();
    }

    private void AddSlashCommands()
    {
        _client.UseSlashCommands().RegisterCommands<SlashCommandsModule>();
    }
}