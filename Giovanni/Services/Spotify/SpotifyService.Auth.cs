using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Giovanni.Services.Spotify
{
    partial class SpotifyService
    {
        private AccessToken _accessToken;
        private AccessToken _userAccessToken;

        public string GetAuthorizeUserLink()
        {
            string scope = "user-read-private user-read-email";
            var clientId = "fec0e2995acd49db8e57db2f6b5ada21";
            var clientSecret = "6f12244191334f579e774ca55c964248";

            return $"https://accounts.spotify.com/authorize?" + HttpUtility
                .UrlEncode($"response_type=code&client_id={clientId}&scope={scope}");
        }

        public async Task GetAccessToken()
        {
            LogService.Log("Creating access token");
            var clientSecret = "6f12244191334f579e774ca55c964248";
            var clientId = "fec0e2995acd49db8e57db2f6b5ada21";
            var credentials = $"{clientId}:{clientSecret}";
            var code =
                "AQDQwD90UqvfXppllOC-p6VM1S0DtL12l7I5ueL8NY3NSq-ncl45-49GO7uLlXL3msfOH6xFqZgIIxvae38j5ZuTE_f7fHePEVLnO5CH9t6UMq16jdB9N6eLAfZ8Y75-byTwlt5zAg6-kGJkbJRAcjqNh4Sjj5vvBRTbucwxJIjPw8BxvUpGFYzCmc56";
            HttpClient client = _httpService.GetClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

            var requestData = new List<KeyValuePair<string, string>>
            {
                new("code", code),
                new("redirect_uri", "http://localhost:8001/"),
                new("grant_type", "authorization_code"),
            };
            var requestBody = new FormUrlEncodedContent(requestData);

            HttpResponseMessage request = await client.PostAsync("https://accounts.spotify.com/api/token", requestBody);
            string response = await request.Content.ReadAsStringAsync();

            _userAccessToken = JsonConvert.DeserializeObject<AccessToken>(response);
            _userAccessToken.created_at = DateTime.Now;
        }

        private async Task GenerateAccessToken()
        {
            LogService.Log("Creating token");
            var clientId = "fec0e2995acd49db8e57db2f6b5ada21";
            var clientSecret = "6f12244191334f579e774ca55c964248";

            var credentials = $"{clientId}:{clientSecret}";
            HttpClient client = _httpService.GetClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

            var requestData = new List<KeyValuePair<string, string>>
                {new KeyValuePair<string, string>("grant_type", "client_credentials")};
            var requestBody = new FormUrlEncodedContent(requestData);

            var request = await client.PostAsync("https://accounts.spotify.com/api/token", requestBody);
            var response = await request.Content.ReadAsStringAsync();

            _accessToken = JsonConvert.DeserializeObject<AccessToken>(response);
            _accessToken.created_at = DateTime.Now;

            await LogService.Log(_accessToken.ToString());
        }

        public async Task<string> GetToken()
        {
            if (_accessToken is null || _accessToken.IsExpired)
            {
                await GenerateAccessToken();
            }

            return _accessToken.access_token;
        }
    }
}