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
        if (arg is not SocketUserMessage message)
        {
            return;
        }

        var authorIsBot = message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot;
        if (authorIsBot) return;

        var prefixPosition = 0;

        if (message.HasCharPrefix('!', ref prefixPosition))
        {
            var context = new SocketCommandContext(_client, message);
            if (message.Embeds.Any()) await message.DeleteAsync();

            Console.WriteLine($"Executing {message.Content}");

            try
            {
                await _commands.ExecuteAsync(context, prefixPosition, _services);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}