using Newtonsoft.Json;

namespace Giovanni.Models.Spotify.DataSource
{
    public class UserDataSource
    {
        [JsonProperty("id")] public string ID;
        [JsonProperty("href")] public string Link;
        [JsonProperty("external_urls")] public ExternalURLsDataSource ExternalUrLs;
    }
}