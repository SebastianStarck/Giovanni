using Newtonsoft.Json;

namespace Giovanni.Models.Spotify.DataSource
{
    public class ArtistDataSource
    {
        [JsonProperty("id")] public string ID;
        [JsonProperty("name")] public string Name;
        [JsonProperty("genres")] public string[] Genres;
        [JsonProperty("href")] public string Link;
        [JsonProperty("external_urls")] public ExternalURLsDataSource ExternalUrLs;
    }
}