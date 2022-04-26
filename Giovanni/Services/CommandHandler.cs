using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IServiceProvider _services;

    public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
    {
        _commands = commands;
        _client = client;
        _services = services;
    }

    public async Task HandleCommandAsync(SocketMessage arg)
    {
        var msg = arg as SocketUserMessage;
        if (msg == null) return;

        bool authorIsBot = msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot;
        if (authorIsBot) return;

        int prefixPosition = 0;

        if (msg.HasCharPrefix('!', ref prefixPosition))
        {
            var context = new SocketCommandContext(_client, msg);

            Console.WriteLine($"Services count: ${_services}");
            var result = await _commands.ExecuteAsync(context, prefixPosition, _services);

            if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                await msg.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}