using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Giovanni.Models;
using Giovanni.Models.Spotify;
using Giovanni.Models.Spotify.DataSource;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Giovanni.Services.Spotify
{
    public partial class SpotifyService
    {
        private readonly HttpService _httpService;
        private readonly CacheService CacheService = new();

        public SpotifyService()
        {
            _httpService = new HttpService("https://api.spotify.com/v1/", GetToken);
        }

        public Task<ArtistDataSource> GetArtistByID(string id)
        {
            return CacheService.GetOrCreateAsync(id, async () =>
                {
                    var request = await _httpService
                        .Get($"artists/{id}");
                    var response = await request.Content.ReadAsStringAsync();
                    var artist = JsonConvert.DeserializeObject<ArtistDataSource>(response);

                    return artist;
                }
            );
        }

        public async Task<Playlist?> GetPlaylistByID(string id)
        {
            var parameters = new Dictionary<string, string>() {{"limit", "50"}};
            var request = await _httpService.Get($"playlists/{id}", parameters);
            var response = await request.Content.ReadAsStringAsync();

            return new Playlist(this, JsonConvert.DeserializeObject<PlaylistDataSource>(response));
        }

        public async Task<Paginated<PlaylistItem>?> GetPlaylistTracks(string id)
        {
            var parameters = new Dictionary<string, string>() {{"limit", "50"}};
            var request = await _httpService
                .Get($"playlists/{id}/tracks", parameters);
            var response = await request.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Paginated<PlaylistItem>>(response);
        }
    }
}