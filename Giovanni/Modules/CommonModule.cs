using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Giovanni.Modules
{
    using Discord.Commands;

    public class CommonModule : ModuleBase<SocketCommandContext>
    {
        private bool _isDeleting = false;

        // ~say hello world -> hello world
        [Command("ping")]
        [Summary("Pong!")]
        public Task SayAsync() => ReplyAsync("!Pong");

        [Command("userinfo")]
        [Summary("Returns info about the current user, or the user parameter, if one passed.")]
        [Alias("user", "whois")]
        public async Task UserInfoAsync([Summary("The (optional) user to get info from")] SocketUser user = null)
        {
            var userInfo = user ?? Context.Client.CurrentUser;
            await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        }

        [Command("embed")]
        public async Task SendRichEmbedAsync()
        {
            var embed = new EmbedBuilder
            {
                // Embed property can be set within object initializer
                Title = "Hello world!",
                Description = "I am a description set by initializer."
            };
            // Or with methods
            embed.AddField("Field title",
                    "Field value. I also support [hyperlink markdown](https://example.com)!")
                // .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer => footer.Text = "I am a footer.")
                .WithColor(Color.Blue)
                .WithTitle("I overwrote \"Hello world!\"")
                .WithDescription("I am a description.")
                .WithUrl("https://example.com")
                .WithImageUrl("https://i.pinimg.com/originals/65/ba/48/65ba488626025cff82f091336fbf94bb.gif")
                .WithCurrentTimestamp();

            //Your embed needs to be built before it is able to be sent
            await ReplyAsync(embed: embed.Build());
        }

        [Command("purge")]
        [Alias("clean")]
        [Summary("Removes X messages from the current channel.")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeAsync(int amount)
        {
            if (!_isDeleting) _isDeleting = true;
            else return;

            var messages = (await Context.Channel
                .GetMessagesAsync(Context.Message, Direction.Before, amount > 0 ? amount : 0)
                .FlattenAsync()).OrderByDescending(message => message.Id);

            var channel = Context.Channel as ITextChannel;
            var count = 0;
            foreach (var message in messages)
            {
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


            Console.WriteLine($"Deleted {count} messages");
            await channel.DeleteMessageAsync(Context.Message);
            _isDeleting = false;
        }
    }
}