using System;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Giovanni.Modules
{
    using Discord.Commands;

    public class CommonModule : ModuleBase<SocketCommandContext>
    {
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
    }
}
