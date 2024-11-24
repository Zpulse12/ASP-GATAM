using Gatam.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions.httpExtensions
{
    public class HttpWrapper : IHttpWrapper
    {
        private readonly HttpClient _httpClient;
        public HttpWrapper(HttpClient httpClient)
        {
             _httpClient = httpClient;
        }
        public async Task<Result<HttpResponseMessage>> SendDeleteWithBody<T>(string url, T body)
        {
            if(url == null) { return Result<HttpResponseMessage>.Fail(new NullReferenceException("parameter url cannot be null")); }
            try
            {
                string jsonPayload = JsonSerializer.Serialize(body);
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Delete, url)
                {
                    Content = content
                };
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if(response.IsSuccessStatusCode) { return Result<HttpResponseMessage>.Ok(response); }
                else
                {
                    return Result<HttpResponseMessage>.Fail(new Exception(response.ReasonPhrase));
                }

            }
            catch (Exception ex)
            {
                return Result<HttpResponseMessage>.Fail(ex);
            }
        }
    }
}
