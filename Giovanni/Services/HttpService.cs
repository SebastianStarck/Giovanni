using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Giovanni.Services
{
    public class HttpService
    {
        private static readonly CacheService CacheService = new();
        private static readonly HttpClient Client = new(new HttpClientHandler(), false);
        private readonly string _baseUrl;
        private string _token;
        private readonly Func<Task<string>> _getToken;

        public HttpClient GetClient() => Client;


        public HttpService(string baseUrl, Func<Task<string>> getToken)
        {
            _baseUrl = baseUrl;
            _getToken = getToken;
        }

        private async Task RenewToken() => _token = await _getToken();

        private void SetToken() =>
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);

        public async Task<HttpResponseMessage> Post(string route)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>());
            var response = await Client.PostAsync("http://www.example.com/recepticle.aspx", content);

            return response;
        }

        public async Task<HttpResponseMessage> Get(string route, Dictionary<string, string> parameters = null)
        {
            Client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", await _getToken());

            if (parameters is not null)
            {
                foreach ((string header, string value) in parameters)
                {
                    Client.DefaultRequestHeaders.Add(header, value);
                }
            }

            var url = _baseUrl + route;
            LogService.Log($"GET {url}");

            return await CacheService.GetOrCreateAsync(url, () => Client.GetAsync(url));
        }

        public static async Task<HttpResponseMessage> GetStatic(string route,
            Dictionary<string, string> parameters = null)
        {
            if (parameters is not null)
            {
                foreach ((string header, string value) in parameters)
                {
                    Client.DefaultRequestHeaders.Add(header, value);
                }
            }

            var url = route;
            LogService.Log($"GET {url}");

            return await CacheService.GetOrCreateAsync(url, () => Client.GetAsync(url));
        }

        public static async Task<T> GetStatic<T>(string route,
            Dictionary<string, string> parameters = null)
        {
            if (parameters is not null)
            {
                foreach ((var header, var value) in parameters)
                {
                    Client.DefaultRequestHeaders.Add(header, value);
                }
            }

            var url = route;
            LogService.Log($"GET {url}");

            return await CacheService.GetOrCreateAsync(url, async () =>
            {
                var obj = await (await Client.GetAsync(url)).Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(obj);
            });
        }
    }
}