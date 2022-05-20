using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Giovanni.Common;

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
        if (arg is not SocketUserMessage message) return;

        var authorIsBot = message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot;
        if (authorIsBot) return;

        var prefixPosition = 0;
        if (!message.HasCharPrefix('!', ref prefixPosition)) return;

        try
        {
            var context = new SocketCommandContext(_client, message);
            string messageContent = message.Content;

            if (messageContent.Contains("comandos"))
            {
                ExplainCommands(context);

                return;
            }

            if (messageContent.Contains("help"))
            {
                var (_, commandName) = messageContent.Split(" ");
                ExplainCommand(commandName, context);

                return;
            }

            if (message.Embeds.Any()) await message.DeleteAsync();

            Console.WriteLine($"Executing {messageContent}");

            _commands.ExecuteAsync(context, prefixPosition, _services);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void ExplainCommand(string commandName, SocketCommandContext context)
    {
        var command = _commands.Commands.FirstOrDefault(command => command.Name == commandName);

        if (command is null) context.Channel.SendMessageAsync("No encontre el comando buscado :(");

        var embed = new EmbedBuilder().WithColor(Color.Purple).AddField(command.Name, command.Summary ?? "???")
            .WithDescription(command.GetDescription());

        if ((bool) command.Parameters?.Any())
        {
            var strigifiedParams = command.Parameters.Aggregate("",
                (result, current) => $"{result}\n{current.Name}: {current.Type.Name}");
            embed.AddField("Params",
                $"```yml\n{strigifiedParams}```");
        }

        context.Channel.SendMessageAsync(embed: embed.Build());
    }

    private void ExplainCommands(SocketCommandContext context)
    {
        var embed = new EmbedBuilder().WithColor(Color.Purple);
        var commands = _commands.Commands.OrderBy(command => command.Name);

        foreach (var command in commands)
        {
            embed.AddField(command.Name, command.Summary ?? "???");

            if (command.Parameters.Any())
            {
                var strigifiedParams = command.Parameters.Aggregate("",
                    (result, current) => $"{result}\n{current.Name}: {current.Type.Name}");
                embed.AddField("Params",
                    $"```yml\n{strigifiedParams}```");
            }
        }

        context.Channel.SendMessageAsync(embed: embed.Build());
    }

    public async Task HandleButtonAsync(SocketMessageComponent arg)
    {
        // Console.WriteLine(arg.Data.CustomId);

        foreach (var command in this._commands.Commands)
        {
            Console.WriteLine(command.Name);
            Console.WriteLine(command.Summary);
            Console.WriteLine(command.Parameters);
            Console.WriteLine(
                string.Join(", ",
                    command.Parameters.Select(command => command.Name)));
        }

        await arg.RespondAsync($"Button clicked by {arg.User.Username}");

        return;
    }
}