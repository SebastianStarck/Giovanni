using Newtonsoft.Json;

namespace Giovanni.Models.Spotify.DataSource
{
    public class TrackDataSource
    {
        [JsonProperty("id")] public string ID;
        [JsonProperty("name")] public string Name;
        [JsonProperty("popularity")] public int Popularity;
        [JsonProperty("album")] public AlbumDataSource Album;
        [JsonProperty("artists")] public ArtistDataSource[] Artists;
        [JsonProperty("duration")] public int Duration;
        [JsonProperty("external_urls")] public ExternalURLsDataSource ExternalURLs;
        [JsonProperty("track_number")] public int TrackNumber;
        [JsonProperty("href")] public string Link;

        public override string ToString() => Name;
    }
}