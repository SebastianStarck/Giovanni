using Newtonsoft.Json;

namespace Giovanni.Models.Spotify.DataSource
{
    public class UserDataSource
    {
        [JsonProperty("id")] public string ID;
        [JsonProperty("href")] public string APILink;
        [JsonProperty("external_urls")] public ExternalURLsDataSource ExternalUrLs;
        [JsonProperty("display_name")] public string Name;
    }
}