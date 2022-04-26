using Newtonsoft.Json;

namespace Giovanni.Models.Spotify.DataSource
{
    public class ExternalURLsDataSource
    {
        [JsonProperty("spotify")] public string Spotify;
    }
}