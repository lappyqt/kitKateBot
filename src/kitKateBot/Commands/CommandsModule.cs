using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace kitKateBot.Commands;

public class CommandsModule : BaseCommandModule
{
    [Command("Ping")]
    public async Task HandlePingCommand(CommandContext context) 
    {
        await context.RespondAsync("Pong!");
    }
}