﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Gatam.Application.Extensions.EnvironmentHelper;

namespace Gatam.WebAPI.Extensions.Filters
{
    public class IsAuthenticatedApiKey : IActionFilter
    {
        private readonly EnvironmentWrapper _environmentWrapper;

        public IsAuthenticatedApiKey(EnvironmentWrapper environmentWrapper)
        {
            _environmentWrapper = environmentWrapper;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("X-Api-Key", out var apiKey) || string.IsNullOrEmpty(apiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (apiKey != _environmentWrapper.APIKEY)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }

}
