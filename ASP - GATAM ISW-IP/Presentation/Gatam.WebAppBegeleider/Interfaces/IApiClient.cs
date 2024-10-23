using Gatam.WebAppBegeleider.Extensions;

namespace Gatam.WebAppBegeleider.Interfaces
{
    public interface IApiClient
    {
        HttpClient _httpClient { get; }
        public Task<HttpResponseMessage> MakeAsyncRequest(HttpRequestMessage req);
        Task<T?> GetFromJsonAsync<T>(string requestUri);
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T content);
        Task<HttpResponseMessage> PutAsJsonAsync<T>(string requestUri, T content);
        Task<HttpResponseMessage> DeleteAsync(string requestUri);

    }
}
