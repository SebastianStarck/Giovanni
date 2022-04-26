using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Giovanni.Models.Spotify
{
    public class Track
    {
        [JsonProperty("id")] public string ID;
        [JsonProperty("name")] public string Name;
        [JsonProperty("popularity")] public int Popularity;
        [JsonProperty("album")] public Album Album;
        [JsonProperty("artists")] public Artist[] Artists;
        [JsonProperty("duration")] public int Duration;
        [JsonProperty("external_urls")] public ExternalURLs ExternalURLs;
        [JsonProperty("track_number")] public int TrackNumber;
        [JsonProperty("href")] public string Link;

        public override string ToString() => Name;
    }
}