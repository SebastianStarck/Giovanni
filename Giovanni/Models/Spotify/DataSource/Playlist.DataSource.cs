using Newtonsoft.Json;

namespace Giovanni.Models.Spotify.DataSource
{
    public class PlaylistDataSource
    {
        //[JsonProperty("followers")] public Followers Followers;
        [JsonProperty("id")] public string ID;
        [JsonProperty("public")] public bool IsPublic;
        [JsonProperty("collaborative")] public bool Collaborative;
        [JsonProperty("owner")] public UserDataSource Owner;
        [JsonProperty("name")] public string Name;
        [JsonProperty("description")] public string Description;
        [JsonProperty("images")] public ImageDataSource[] Images = System.Array.Empty<ImageDataSource>();
        [JsonProperty("snapshot_id")] public string SnapshotID;
        [JsonProperty("external_urls")] public ExternalURLsDataSource ExternalUrLs;
        [JsonProperty("tracks")] public Paginated<PlaylistItemDataSource> PaginatedPlaylistItems;
    }
}