using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace kitKateBot.Commands;

public class SlashCommandsModule : ApplicationCommandModule
{
    [SlashCommand("ping", "Returns a pong!!!")]
    public async Task HandlePongCommand(InteractionContext context)
    {
        await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Pong!"));
    }
}