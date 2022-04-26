using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Giovanni.Services
{
    public class HttpService
    {
        public static readonly HttpClient Client = new HttpClient();
        private readonly string _baseURL;
        private string _token;
        private Func<Task<string>> _getToken;

        public HttpClient GetClient() => Client;

        public HttpService()
        {
        }

        public HttpService(string baseUrl, Func<Task<string>> getToken)
        {
            _baseURL = baseUrl;
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

            LogService.Log($"GET {_baseURL + route}");
            var response = await Client.GetAsync(_baseURL + route);

            Console.WriteLine(response.Headers);
            return response;
        }
    }
}