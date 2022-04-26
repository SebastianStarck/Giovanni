using Newtonsoft.Json;

namespace Giovanni.Models.Spotify
{
    public class PlaylistItem
    {
        [JsonProperty("added_at")] public string AddedAt;
        [JsonProperty("added_by")] public User AddedBy;
        [JsonProperty("track")] public Track Track;
    }
}