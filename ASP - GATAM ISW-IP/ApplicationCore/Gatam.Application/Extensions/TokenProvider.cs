using Gatam.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions
{
    public class TokenProvider : ITokenProvider
    {

        private readonly IHttpContextAccessor _httpContextAccess;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccess = httpContextAccessor;
        }
        public async Task<string> GetToken()
        {
            return await _httpContextAccess.HttpContext.GetTokenAsync("access_token");
        }
    }
}
