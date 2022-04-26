#nullable enable
using Newtonsoft.Json;

namespace Giovanni.Models.Spotify
{
    public class Image
    {
        [JsonProperty("url")] public string? URL;
        [JsonProperty("height")] public int? Height;
        [JsonProperty("width")] public int? Width;
    }
}