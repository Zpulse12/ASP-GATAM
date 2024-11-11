using System.Net.Http.Headers;

namespace Gatam.WebAppBegeleider.Extensions
{
    public class HeaderHandler : DelegatingHandler
    {
        private readonly TokenService _tokenService;

        public HeaderHandler(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token = await _tokenService.GetBearerTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
