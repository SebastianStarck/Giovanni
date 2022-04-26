using System;
using System.Linq;
using System.Threading.Tasks;
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
            string listID = Util.GetIDFromURL(url);
            Playlist list = await SpotifyService.GetPlaylistByID(listID);

            if (list is null)
            {
                await ReplyAsync("List not found");

                return;
            }

            await ReplyAsync("Processing list!");
            var genres = (await list.GetGenresCount());
            var sortedGenres = genres.OrderByDescending(x => x.Value);
            await ReplyAsync(list.ToString());
            await ReplyAsync(list.GetArtists()
                .Aggregate("```\nArtists: \n", (result, artist) => result + $"- {artist}\n") + "```");
            await ReplyAsync(sortedGenres.Take(5).Aggregate("```\nGenres:\n",
                (result, pair) => result + $"{pair.Key}: {pair.Value}\n") + "```");
        }
    }
}