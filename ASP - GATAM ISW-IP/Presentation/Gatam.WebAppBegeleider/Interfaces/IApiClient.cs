using Gatam.WebAppBegeleider.Extensions;

namespace Gatam.WebAppBegeleider.Interfaces
{
    public interface IApiClient
    {
        HttpClient _httpClient { get; }
        TokenService _tokenService { get; set; }
        public Task<HttpResponseMessage> MakeAsyncRequest(HttpRequestMessage req);
    }
}
