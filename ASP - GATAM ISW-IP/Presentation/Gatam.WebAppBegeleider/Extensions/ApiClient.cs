using Gatam.WebAppBegeleider.Interfaces;

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
            return response;
        }
        public async Task<HttpResponseMessage> PatchAsJsonAsync<T>(string requestUri, T content)
        {
            var response = await _httpClient.PatchAsJsonAsync(requestUri, content);
            return response;
        }
        public async Task<T?> GetFromJsonAsync<T>(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            var response = await _httpClient.DeleteAsync(requestUri);
            return response;
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string requestUri, T content)
        {
            var response = await _httpClient.PutAsJsonAsync(requestUri, content);
            return response;
        }
        public async Task<HttpResponseMessage> PutAsync(string requestUri)
        {
            return await _httpClient.PutAsync(requestUri, null);
        }
        public async Task<HttpResponseMessage> PostAsync(string requestUri)
        {
            return await _httpClient.PostAsync(requestUri, null);
        }
    }
}
