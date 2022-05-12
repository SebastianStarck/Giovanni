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

    public class SpotifyModule : ModuleBase<SocketCommandContext>
    {
        public SpotifyService SpotifyService { get; set; }

        // [Command("login-spotify")]
        // public async Task LoginSpotify()
        // {
        //     await ReplyAsync(SpotifyService.GetAuthorizeUserLink());
        // }

        [Command("contar")]
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

            // await ReplyAsync(list.ToString());
            // await ReplyAsync(list.GetArtists()
            // .Aggregate("```\nArtists: \n", (result, artist) => result + $"- {artist}\n") + "```");
            // await ReplyAsync(sortedGenres.Take(5).Aggregate("```\nGenres:\n",
            // (result, pair) => result + $"{pair.Key}: {pair.Value}\n") + "```");

            var owner = list.Owner;
            var embed = new EmbedBuilder()
                .AddField("Songs", list.GetSongsOverview())
                .AddField("Owner", $"[{owner.Name}]({owner.ExternalUrLs.Spotify})")
                .AddField("Top genres", string.Join(", ", sortedGenres.Take(3).Select(entry => entry.Key)))
                .WithThumbnailUrl(list.Image)
                //.WithFooter(footer => footer.Text = "I am a footer.")
                .WithColor(Color.Blue)
                .WithTitle($"{list.Name}")
                .WithUrl(list.Link)
                .WithCurrentTimestamp();

            //Your embed needs to be built before it is able to be sent
            await ReplyAsync(embed: embed.Build());
        }
    }
}