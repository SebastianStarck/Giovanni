using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Giovanni.Services;
using Newtonsoft.Json;

namespace Giovanni.Models.Spotify
{
    public class Playlist
    {
        //[JsonProperty("followers")] public Followers Followers;
        [JsonProperty("id")] public string ID;
        [JsonProperty("public")] public bool IsPublic;
        [JsonProperty("collaborative")] public bool Collaborative;
        [JsonProperty("owner")] public User Owner;
        [JsonProperty("name")] public string Name;
        [JsonProperty("description")] public string Description;
        [JsonProperty("images")] public Image[] Images;
        [JsonProperty("snapshot_id")] public string SnapshotID;
        [JsonProperty("external_urls")] public ExternalURLs ExternalUrLs;
        [JsonProperty("tracks")] public Paginated<PlaylistItem> PaginatedPlaylistItems;

        private HashSet<Artist> _artists;
        private Dictionary<string, int> _genresCount;

        public override string ToString() => $"List name: {Name} \n Tracks: {PaginatedPlaylistItems.Total}";

        public async Task<Dictionary<string, int>> GetGenresCount() =>
            _genresCount ?? await CalculateGenresCount();

        private async Task<Dictionary<string, int>> CalculateGenresCount()
        {
            _genresCount = new Dictionary<string, int>();

            foreach (Artist artist in GetArtists())
            {
                using var response =
                    await HttpService.Client.GetAsync($"https://api.spotify.com/v1/artists/{artist.ID}");

                var fetchedArtist = JsonConvert.DeserializeObject<Artist>(await response.Content.ReadAsStringAsync());
                foreach (string genre in fetchedArtist.GetGenres())
                {
                    _genresCount[genre] = _genresCount.GetValueOrDefault(genre) + 1;
                }
            }

            return _genresCount;
        }

        public HashSet<Artist> GetArtists() => _artists ?? CalculateArtists();
        public HashSet<Artist> GetArtistsWith() => _artists ?? CalculateArtists();

        private HashSet<Artist> CalculateArtists()
        {
            _artists = new HashSet<Artist>();

            foreach (PlaylistItem item in PaginatedPlaylistItems.Items)
            foreach (Artist artist in item.Track.Artists)
                _artists.Add(artist);

            _artists = _artists.OrderBy(artist => artist.ToString()).ToHashSet();
            
            return _artists;
        }
    }
}