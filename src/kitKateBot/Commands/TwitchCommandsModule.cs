using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using kitKateBot.Services;

namespace kitKateBot.Commands;

[SlashCommandGroup("Twitch", "Команды для работы с Twitch")]
public class TwitchCommandsModule : ApplicationCommandModule
{
    private readonly ITwitchService _service;

    public TwitchCommandsModule(ITwitchService service)
    {
        _service = service;
    }

    [SlashCommand("user", "Возвращает информацию о пользователе на Twitch")]
    public async Task HandleUserCommand(InteractionContext context, [Option("логин", "Имя пользователя на Twitch")] string login)
    {
        await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

        var response = await _service.GetUser(login);

        if (response.Data.Length <= 0) 
        {
            await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(new DiscordEmbedBuilder
            {
                Title = $"Аккаунт {login} не найден",
                Color = DiscordColor.White, 
                ImageUrl = "https://media.tenor.com/BbSkyx3DaEgAAAAC/goma-sad.gif"
            }));
        }

        var user = response.Data[0];

        var message = new DiscordEmbedBuilder
        {
            Title = $"Twitch профиль {user.DisplayName}",
            Color = DiscordColor.Purple,
            ImageUrl = user.ProfileImageUrl
        };

        message.AddField("ID", user.Id);
        message.AddField("Отображаемое имя", user.DisplayName);
        message.AddField("Создан", DateTime.Parse(user.CreatedAt).ToString("dd-MM-yyyy HH:mm"));
        message.AddField("Описание", string.IsNullOrEmpty(user.Description) ? "Отсутствует" : user.Description);
        message.AddField("Тип вещателя", string.IsNullOrEmpty(user.BroadcasterType) ? "Стандартный" : user.BroadcasterType);

        var linkButton = new DiscordLinkButtonComponent($"https://twitch.tv/{login}", "Открыть на Twitch");

        await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(message).AddComponents(linkButton));
    }
}