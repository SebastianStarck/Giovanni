using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Giovanni.Services;

namespace Giovanni.Modules
{
    using Discord.Commands;

    public class DigestorModule : ModuleBase<SocketCommandContext>
    {
        private readonly DatabaseService _databaseService;
        private readonly UsersService _usersService;

        public DigestorModule(DatabaseService databaseService, UsersService usersService)
        {
            _databaseService = databaseService;
            _usersService = usersService;
        }

        // ~say hello world -> hello world
        [Command("digest")]
        [Summary("Digest a given channel")]
        public Task DigestAsync()
        {
            var message = Context.Message;
            var targetChannel = message.MentionedChannels.FirstOrDefault() ?? message.Channel as SocketGuildChannel;
            return ReplyAsync(
                $"Channel name: {targetChannel?.Name ?? "no channel"} \n Channel id: {targetChannel?.Id}");
        }
    }
}