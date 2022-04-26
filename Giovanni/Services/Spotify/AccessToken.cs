using System;

namespace Giovanni.Services.Spotify
{
    internal class AccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public long expires_in { get; set; }
        public DateTime created_at { get; set; }
        public DateTime ExpirationDate => created_at.AddSeconds(expires_in);
        public bool IsExpired => ExpirationDate < DateTime.Now;

        public override string ToString() => $"{access_token}: expires {ExpirationDate}";
    }
}