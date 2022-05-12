using System;
using System.Linq;
using System.Threading.Tasks;
using Giovanni.Models.Spotify.DataSource;
using Giovanni.Services;
using Giovanni.Services.Spotify;
using Newtonsoft.Json;

namespace Giovanni.Models.Spotify
{
    public class Artist
    {
        private readonly SpotifyService _spotifyService;

        public readonly string ID;
        private ArtistDataSource _dataSource;

        public Artist(SpotifyService spotifyService, ArtistDataSource dataSource)
        {
            _spotifyService = spotifyService;

            if (dataSource is null) return;

            ID = dataSource.ID;
            _dataSource = dataSource;
        }

        public async Task<string[]> GetGenres(int limit = 0)
        {
            if (_dataSource is null) return Array.Empty<string>();
            if (_dataSource.Genres is null) await ReFetchDatasource();

            return limit > 0 ? _dataSource.Genres.Take(limit).ToArray() : _dataSource.Genres;
        }

        private async Task ReFetchDatasource() => _dataSource = await _spotifyService.GetArtistByID(ID);


        public override string ToString() => _dataSource.Name;

        public override bool Equals(object? obj)
        {
            if (obj is Artist artist) return artist._dataSource.Name == _dataSource.Name;

            return false;
        }

        protected bool Equals(Artist other) => ID == other.ID;

        public override int GetHashCode() => (ID != null ? ID.GetHashCode() : 0);
    }
}