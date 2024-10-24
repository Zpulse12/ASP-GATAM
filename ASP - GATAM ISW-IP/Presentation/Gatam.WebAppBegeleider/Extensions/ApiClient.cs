using Auth0.ManagementApi.Models;
using Gatam.WebAppBegeleider.Interfaces;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Gatam.WebAppBegeleider.Extensions
{
    public class ApiClient : IApiClient
    {
        public HttpClient _httpClient { get; }
        public ApiClient(HttpClient httpClient) { 
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> MakeAsyncRequest(HttpRequestMessage req)
        {
            return await _httpClient.SendAsync(req);
        }
        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T content)
        {
            var response = await _httpClient.PostAsJsonAsync(requestUri, content);
            response.EnsureSuccessStatusCode();
            return response;
        }
        public async Task<T?> GetFromJsonAsync<T>(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string requestUri, T content)
        {
            var response = await _httpClient.PutAsJsonAsync(requestUri, content);
            response.EnsureSuccessStatusCode();
            return response;
        }
        public async Task<T?> GetAllUsersFromAuth0<T>(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
