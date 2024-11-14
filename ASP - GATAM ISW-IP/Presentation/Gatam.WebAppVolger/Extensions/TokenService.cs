using Auth0.AspNetCore.Authentication;
using Gatam.WebAppBegeleider.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace Gatam.WebAppVolger.Extensions
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        /// <summary>
        /// Verkrijg de Bearer Token in de huidige context.
        /// </summary>
        /// <returns>String | de token of lege string</returns>
        public async Task<string> GetBearerTokenAsync()
        {
            if (_contextAccessor.HttpContext == null) {
                throw new NullReferenceException("HTTP CONTEXT IS NULL. WTF");
            } else
            {

                AuthenticateResult authenticationResult = await _contextAccessor.HttpContext.AuthenticateAsync(Auth0Constants.AuthenticationScheme);
                if (authenticationResult.Succeeded && authenticationResult.Properties.Items.TryGetValue(".Token.access_token", out string? token)) {
                    return token ?? "";
                }
                return "";
            }
        }
    }
}
