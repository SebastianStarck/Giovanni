using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Giovanni.Common;
using Giovanni.Modules.Spotify;
using Giovanni.Services;
using Giovanni.Services.Database.Tables;

namespace Giovanni.Modules
{
    using Discord.Commands;

    public class CommonModule : ModuleBase<SocketCommandContext>
    {
        private DatabaseService _databaseService;

        public CommonModule(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        private bool _isDeleting = false;

        [Command("ping")]
        [Summary("Pong!")]
        public Task SayAsync() => ReplyAsync("!Pong");

        [Command("purge")]
        [Alias("clean")]
        [Summary("Removes X messages from the current channel.")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeAsync(int amount, params string[] args)
        {

            _databaseService.GetMany<Playlist>();
            return;
            if (!_isDeleting) _isDeleting = true;
            else return;

            var (disposeBotMessage, prefix, botOnly) = new PurgeAsyncParams(args);

            var messages = (await Context.Channel
                .GetMessagesAsync(Context.Message, Direction.Before)
                .FlattenAsync()).OrderByDescending(message => message.Id).Where(message =>
            {
                var passesPrefixRule = prefix.IsEmpty() || message.Content.HasPrefix(prefix);
                var passesBotRule = !botOnly || !message.Author.IsBot;

                return passesBotRule && passesPrefixRule;
            });

            if (!messages.Any()) return;

            var channel = Context.Channel as ITextChannel;
            var count = 0;

            var deleteMessage = await channel.SendMessageAsync(":gear: Deleting messages...");
            await channel.DeleteMessageAsync(Context.Message);

            foreach (var message in messages)
            {
                if (count == amount) break;
                Console.WriteLine($"Deleting message \"{message.Content}\"");
                try
                {
                    await channel.DeleteMessageAsync(message);
                    count++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                await Task.Delay(1000);
            }

            if (!disposeBotMessage)
                await deleteMessage.ModifyAsync(properties =>
                    properties.Content = $":white_check_mark: Deleted {count} messages");
            else await deleteMessage.DeleteAsync();

            _isDeleting = false;
        }

        [Command("purge-bulk")]
        [Alias("clean-bulk")]
        [Summary("Removes X messages at once from the current channel.")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeBulkAsync(int amount, params string[] args)
        {
            if (!_isDeleting) _isDeleting = true;
            else return;

            var (_, prefix, botOnly) = new PurgeAsyncParams(args);

            var messages = (await Context.Channel
                .GetMessagesAsync(Context.Message, Direction.Before)
                .FlattenAsync()).OrderByDescending(message => message.Id).Where(message =>
            {
                var passesDateRule = message.CreatedAt > DateTimeOffset.Now.AddDays(-15);
                var passesPrefixRule = prefix.IsEmpty() || message.Content.HasPrefix(prefix);
                var passesBotRule = !botOnly || !message.Author.IsBot;

                return passesBotRule && passesPrefixRule && passesDateRule;
            });

            if (!messages.Any()) return;

            var channel = Context.Channel as ITextChannel;
            var count = 0;

            await channel.DeleteMessageAsync(Context.Message);
            await channel.DeleteMessagesAsync(messages);

            _isDeleting = false;
        }
    }
}