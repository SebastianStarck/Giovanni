using Newtonsoft.Json;

namespace Giovanni.Models.Spotify
{
    public class Album
    {
        [JsonProperty("id")] public string ID;
        [JsonProperty("artists")] public Artist[] Artists;
        [JsonProperty("external_urls")] public ExternalURLs ExternalURLs;
        [JsonProperty("href")] public string Link;
        [JsonProperty("images")] public object[] Images;
        [JsonProperty("name")] public string Name;
        [JsonProperty("release_date")] public string ReleaseDate;
        [JsonProperty("total_tracks")] public int TotalTracks;

        public override string ToString() => Name;
    }
}