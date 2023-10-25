using kitKateBot.Persistence;
using kitKateBot.Persistence.Repositories.Impl;
using kitKateBot.Services;
using kitKateBot.Services.Impl;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<ApplicationContext>(x => {
    x.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

builder.Services.AddScoped<ITwitchService, TwitchService>();
builder.Services.AddScoped<IAuthorizationHistoryRepository, AuthorizationHistoryRepository>();

builder.Services.AddScoped<DiscordClientService>();

var app = builder.Build();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var discordService = services.GetRequiredService<DiscordClientService>();

await Task.WhenAll(
    discordService.RunAsync(),
    app.RunAsync()
);