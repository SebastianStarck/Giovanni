using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
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
        var message = arg as SocketUserMessage;
        if (message == null) return;

        var authorIsBot = message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot;
        if (authorIsBot) return;

        var prefixPosition = 0;

        if (message.HasCharPrefix('!', ref prefixPosition))
        {
            var context = new SocketCommandContext(_client, message);
            if (message.Embeds.Any()) await message.DeleteAsync();
            _commands.ExecuteAsync(context, prefixPosition, _services);
            //
            // if (result.Error is not null) Console.Write(result.Error);
            // if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
            //     await msg.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}