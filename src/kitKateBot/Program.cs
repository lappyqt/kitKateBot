using kitKateBot.Services.Impl;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<DiscordClientService>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok());

await app.RunAsync();