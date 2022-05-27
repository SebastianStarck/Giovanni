using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Giovanni.Common;
using Giovanni.Models.Spotify;
using Giovanni.Services.Spotify;

namespace Giovanni.Modules
{
    using Discord.Commands;

    [Name("Spotify")]
    public class SpotifyModule : ModuleBase<SocketCommandContext>
    {
        public SpotifyService SpotifyService { get; set; }

        // [Command("login-spotify")]
        // public async Task LoginSpotify()
        // {
        //     await ReplyAsync(SpotifyService.GetAuthorizeUserLink());
        // }

        [Name("Parse Spotify Playlist")]
        [Command("parse-playlist")]
        public async Task ParsePlaylist(string url)
        {
            var listID = Util.GetIDFromURL(url);
            var list = await SpotifyService.GetPlaylistByID(listID);

            if (list is null)
            {
                await ReplyAsync("List not found");

                return;
            }

            var genres = (await list.GetGenresCount());
            var sortedGenres = genres.OrderByDescending(x => x.Value);

            var owner = list.Owner;
            var builder = new ComponentBuilder().WithButton("I'm a button, click me!", "test-button");
            var embed = new EmbedBuilder()
                .AddField("Songs", list.GetSongsOverview())
                .AddField("Owner", $"[{owner.Name}]({owner.ExternalUrLs.Spotify})", true)
                .AddField("Top genres", string.Join(", ", sortedGenres.Take(3).Select(entry => entry.Key)))
                .WithThumbnailUrl(list.Image)
                .WithFooter(footer => footer.Text = "I am a footer.")
                .WithColor(Color.Blue)
                .WithTitle($"{list.Name}")
                .WithUrl(list.Link)
                .WithCurrentTimestamp();

            await ReplyAsync(embed: embed.Build(), components: builder.Build());
            await Context.Message.DeleteAsync();
        }
    }
}