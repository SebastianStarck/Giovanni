using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Giovanni.Models.Spotify.DataSource;
using Giovanni.Services.Spotify;

namespace Giovanni.Models.Spotify
{
    public class Playlist
    {
        //[JsonProperty("followers")] public Followers Followers;
        private readonly SpotifyService _spotifyService;

        public string ID;
        private PlaylistDataSource _dataSource;

        private HashSet<Artist> _artists;
        private Dictionary<string, int> _genresCount;

        public string Name => _dataSource.Name;
        public object Count => _dataSource.PaginatedPlaylistItems.Total;
        public string Image => _dataSource.Images[0]?.URL;
        public UserDataSource Owner => _dataSource.Owner;
        public string Link => _dataSource.ExternalUrLs.Spotify;

        public Playlist(SpotifyService spotifyService, PlaylistDataSource dataSource)
        {
            ID = dataSource.ID;
            _spotifyService = spotifyService;
            _dataSource = dataSource;
        }

        public override string ToString() =>
            $"List name: {_dataSource.Name} \n Tracks: {_dataSource.PaginatedPlaylistItems.Total}";

        public async Task<Dictionary<string, int>> GetGenresCount() =>
            _genresCount ?? await CalculateGenresCount();

        private async Task<Dictionary<string, int>> CalculateGenresCount()
        {
            _genresCount = new Dictionary<string, int>();

            foreach (var artist in GetArtists())
            {
                foreach (string genre in await artist.GetGenres())
                {
                    _genresCount[genre] = _genresCount.GetValueOrDefault(genre) + 1;
                }
            }

            return _genresCount;
        }

        public HashSet<Artist> GetArtists() => _artists ?? CalculateArtists();

        private HashSet<Artist> CalculateArtists()
        {
            _artists = new HashSet<Artist>();

            foreach (var item in _dataSource.PaginatedPlaylistItems.Items)
            {
                foreach (var artist in item.Track.Artists)
                {
                    _artists.Add(new Artist(_spotifyService, artist));
                }
            }

            _artists = _artists.OrderBy(artist => artist.ToString()).ToHashSet();

            return _artists;
        }

        public string GetSongsOverview(int maxCharacters = 50)
        {
            var stringfiedSongs = "";
            var stringfiedSongsCount = 0;

            foreach (var item in _dataSource.PaginatedPlaylistItems.Items)
            {
                if (stringfiedSongs.Length + item.Track.Name.Length > maxCharacters) break;
                if (stringfiedSongsCount > 0) stringfiedSongs += ", ";

                stringfiedSongs += $"{item.Track.Name}";
                stringfiedSongsCount++;
            }

            var remainingSongsCount = _dataSource.PaginatedPlaylistItems.Total - stringfiedSongsCount;

            return $"{stringfiedSongs} {(remainingSongsCount > 0 ? $"and {remainingSongsCount} more..." : "")}";
        }
    }
}