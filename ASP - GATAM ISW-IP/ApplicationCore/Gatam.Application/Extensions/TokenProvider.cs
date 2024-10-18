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
    public class TokenProvider /*: ITokenProvider*/
    {
        public string? accessToken { get; set; }

        //public async Task<string> GetToken()
        //{
        //    var httpContext = _httpContextAccess.HttpContext;

        //    if (httpContext == null)
        //    {
        //        throw new InvalidOperationException("HttpContext is not available.");
        //    }

        //    var TOKEN = await httpContext.GetTokenAsync("access_token") ?? "";
        //    return TOKEN;
        //}
    }
}
