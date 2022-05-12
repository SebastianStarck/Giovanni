using Newtonsoft.Json;

namespace Giovanni.Models.Spotify.DataSource
{
    public class PlaylistItemDataSource
    {
        [JsonProperty("added_at")] public string AddedAt;
        [JsonProperty("added_by")] public UserDataSource AddedBy;
        [JsonProperty("track")] public TrackDataSource Track;

        public override string ToString() => Track.Name;

        public void Deconstruct(out TrackDataSource track)
        {
            track = Track;
        }
    }
}