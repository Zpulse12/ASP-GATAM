using Gatam.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions.Delegates
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ITokenProvider _tokenProvider;

        public AuthHeaderHandler(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenProvider.GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
