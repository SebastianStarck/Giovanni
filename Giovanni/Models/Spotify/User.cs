using Newtonsoft.Json;

namespace Giovanni.Models.Spotify
{
    public class User
    {
        [JsonProperty("id")] public string ID;
        [JsonProperty("href")] public string Link;
        [JsonProperty("external_urls")] public ExternalURLs ExternalUrLs;
    }
}