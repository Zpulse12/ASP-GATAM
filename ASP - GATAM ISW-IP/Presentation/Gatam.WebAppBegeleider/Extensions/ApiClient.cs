using Auth0.ManagementApi.Models;
using Gatam.WebAppBegeleider.Interfaces;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Gatam.WebAppBegeleider.Extensions
{
    public class ApiClient : IApiClient
    {
        public HttpClient _httpClient { get; }
        public TokenService _tokenService { get; set; }
        public ApiClient(HttpClient httpClient, TokenService tokenService) { 
            _httpClient = httpClient;
            _tokenService = tokenService;
            _httpClient.BaseAddress = new Uri("http://webapi:8080/winchester");
        }

        public async Task<HttpResponseMessage> MakeAsyncRequest(HttpRequestMessage req)
        {
            string token = await _tokenService.GetBearerTokenAsync();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            return await _httpClient.SendAsync(req);
        }


        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T content)
        {
            // Add Bearer Token to the request
            string token = await _tokenService.GetBearerTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Serialize content to JSON and send POST request
            var response = await _httpClient.PostAsJsonAsync(requestUri, content);

            // Handle any additional logic, logging, or error checking
            response.EnsureSuccessStatusCode();

            return response;
        }

        // Example for custom GET with JSON response
        public async Task<T?> GetFromJsonAsync<T>(string requestUri)
        {
            // Add Bearer Token to the request
            string token = await _tokenService.GetBearerTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Send GET request and deserialize JSON response
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            // Deserialize response content as JSON
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
