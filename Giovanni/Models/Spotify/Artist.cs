using System;
using System.Linq;
using Newtonsoft.Json;

namespace Giovanni.Models.Spotify
{
    public class Artist
    {
        [JsonProperty("id")] public string ID;
        [JsonProperty("name")] public string Name;
        [JsonProperty("genres")] public string[] _genres;
        [JsonProperty("href")] public string Link;
        [JsonProperty("external_urls")] public ExternalURLs ExternalUrLs;

        public string[] GetGenres(int limit = 0) => limit > 0 ? _genres.Take(limit).ToArray() : _genres;
        public override string ToString() => Name;

        public override bool Equals(object? obj)
        {
            Console.WriteLine(obj);
            Console.WriteLine(obj is Artist);
            if (obj is Artist artist)
            {
                Console.WriteLine($"Other: {artist}, Current: {this}");
                return artist.Name == Name;
            }

            return false;
        }

        protected bool Equals(Artist other)
        {
            return ID == other.ID;
        }

        public override int GetHashCode()
        {
            return (ID != null ? ID.GetHashCode() : 0);
        }
    }
}