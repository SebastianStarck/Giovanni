using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Giovanni.Models;
using Giovanni.Models.Spotify;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Giovanni.Services.Spotify
{
    public partial class SpotifyService
    {
        private readonly HttpService _httpService;
        private readonly IMemoryCache _memoryCache;

        public SpotifyService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _httpService = new HttpService("https://api.spotify.com/v1/", GetToken);
        }

        public async Task<Artist> GetArtistByID(string id)
        {
            var request = await _httpService
                .Get($"artists/{id}");
            var response = await request.Content.ReadAsStringAsync();
            var artist = JsonConvert.DeserializeObject<Artist>(response);

            _memoryCache.Set(id, artist, TimeSpan.FromDays(1));

            return artist;
        }

        public async Task<Playlist?> GetPlaylistByID(string id)
        {
            var parameters = new Dictionary<string, string>() {{"limit", "50"}};
            var request = await _httpService.Get($"playlists/{id}", parameters);
            var response = await request.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Playlist>(response);
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