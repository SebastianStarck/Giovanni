using Newtonsoft.Json;

namespace Giovanni.Models.Spotify.DataSource
{
    public class AlbumDataSource
    {
        [JsonProperty("id")] public string ID;
        [JsonProperty("artists")] public ArtistDataSource[] Artists;
        [JsonProperty("external_urls")] public ExternalURLsDataSource ExternalURLs;
        [JsonProperty("href")] public string Link;
        [JsonProperty("images")] public object[] Images;
        [JsonProperty("name")] public string Name;
        [JsonProperty("release_date")] public string ReleaseDate;
        [JsonProperty("total_tracks")] public int TotalTracks;   
    }
}