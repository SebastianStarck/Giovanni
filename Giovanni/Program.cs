using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Giovanni.Common;
using Giovanni.Services;
using Giovanni.Services.Database.MySQL.Tables;
using Giovanni.Services.Spotify;

class Program
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IServiceProvider _services;

    private readonly CommandHandler _commandHandler;

    static Task Main(string[] args) => new Program().MainAsync();

    private Program()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            //MessageCacheSize = 50,
            //WebSocketProvider = WS4NetProvider.Instance
        });

        _commands = new CommandService(new CommandServiceConfig {LogLevel = LogSeverity.Info});
        _services = ConfigureServices();
        _commandHandler = new CommandHandler(_client, _commands, _services);

        _client.Log += LogService.Log;
        _commands.Log += LogService.Log;
    }

    private static IServiceProvider ConfigureServices()
    {
        var map = new ServiceCollection()
            .AddSingleton<CacheService>()
            .AddSingleton<HttpService>()
            .AddSingleton<DatabaseService>()
            .AddSingleton<SpotifyService>()
            .AddSingleton<UsersService>();

        return map.BuildServiceProvider();
    }

    private async Task MainAsync()
    {
        await InitCommands();

        await _client.LoginAsync(TokenType.Bot, "OTUzODQzMTg4MDQ3Njc5NTU5.YjKdsQ.dr1yBoTQwoXiZp3BMQO9p9F2eCw");
        // Environment.GetEnvironmentVariable("DiscordToken"));
        await _client.StartAsync();
        
        await Task.Delay(Timeout.Infinite);
    }

    private async Task InitCommands()
    {
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        _client.MessageReceived += _commandHandler.HandleCommandAsync;
        _client.ButtonExecuted += _commandHandler.HandleButtonAsync;
    }
}