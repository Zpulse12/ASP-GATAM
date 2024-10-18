using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;

namespace Gatam.WebAppBegeleider.Middleware
{
    public class SetAuthTokenMiddleware
    {

        private readonly RequestDelegate _next;

        public SetAuthTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            System.Diagnostics.Debug.WriteLine("INVOKING SetAuthTokenMiddleware");

            string accessToken = await httpContext.GetTokenAsync("access_token");

            if (!accessToken.IsNullOrEmpty())
            {
                System.Diagnostics.Debug.WriteLine("TOKEN IS -> ",accessToken);
                httpContext.Request.Headers.Authorization = new StringValues(["Bearer", accessToken]);
            }
            await _next(httpContext);
        }
    }
}
